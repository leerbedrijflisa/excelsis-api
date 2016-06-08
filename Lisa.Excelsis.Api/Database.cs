using System;
using System.Threading.Tasks;
using Lisa.Common.TableStorage;
using Lisa.Common.WebApi;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Options;

namespace Lisa.Excelsis.Api
{
    public class Database
    {
        public Database(IOptions<TableStorageSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<IEnumerable<DynamicModel>> FetchAssessments(Sorting sorting = null)
        {
            if (sorting == null)
            {
                CloudTable table = await Connect("Assessments");
                var query = new TableQuery<DynamicEntity>();
                var assessmentsToMap = await table.ExecuteQuerySegmentedAsync(query, null);
                var assessments = assessmentsToMap.Select(a => AssessmentMapper.ToModel(a));
                return assessments;
            }
            else
            {
                CloudTable table = await Connect("Assessments");
                var query = new TableQuery<DynamicEntity>();
                var assessmentsToMap = await table.ExecuteQuerySegmentedAsync(query, null);
                var assessmentsUnordered = assessmentsToMap.Select(a => AssessmentMapper.ToModel(a));
                IList<DynamicModel> assessmentsUnorderedButCasted = new List<DynamicModel>();
                for (int i = 0; i < assessmentsUnordered.Count(); i++)
                {
                    DynamicModel[] assessorsCasted = new DynamicModel[((object[])assessmentsUnordered.ElementAt(i)["assessors"]).Length];
                    for (int j = 0; j < ((object[])assessmentsUnordered.ElementAt(i)["assessors"]).Length; j++)
                    {
                        assessorsCasted[j] = (DynamicModel)((object[])assessmentsUnordered.ElementAt(i)["assessors"]).ElementAt(j);
                    }
                    var assessmentCasted = assessmentsUnordered.ElementAt(i);
                    assessmentCasted["assessors"] = assessorsCasted;
                    assessmentsUnorderedButCasted.Add(assessmentCasted);
                }
                var assessmentsFirstOrdered = (sorting.Sort[0].StartsWith("assessors.")) ? null : (sorting.Order[0] == "asc") ? assessmentsUnorderedButCasted.OrderBy(x => x[sorting.Sort[0]]) : assessmentsUnorderedButCasted.OrderByDescending(x => x[sorting.Sort[0]]);

                Func<string, string, bool, IOrderedEnumerable<DynamicModel>, IOrderedEnumerable<DynamicModel>> orderByAssessor = (order, field, secondPlus, assessmentsToOrder) => {
                    ICollection<DynamicModel> assessmentsOrderedByAssessor = new List<DynamicModel>();
                    var assessorsField = field.Replace("assessors.", "");
                    if (secondPlus == false)
                    {
                        foreach (var assessment in assessmentsToOrder)
                        {
                            var assessmentOrdered = assessment;
                            assessmentOrdered["assessors"] = (((DynamicModel[])assessment["assessors"]).OrderBy(x => x[assessorsField]));
                            assessmentsOrderedByAssessor.Add(assessmentOrdered);
                        }
                        if (order == "asc")
                        {
                            assessmentsOrderedByAssessor.OrderBy(x => ((DynamicModel[])x["assessors"])[0][assessorsField]);
                        }
                        else
                        {
                            assessmentsOrderedByAssessor.OrderByDescending(x => ((DynamicModel[])x["assessors"])[0][assessorsField]);
                        }
                    }
                    else
                    {
                        foreach (var assessment in assessmentsToOrder)
                        {
                            var assessmentOrdered = assessment;
                            assessmentOrdered["assessors"] = (((DynamicModel[])assessment["assessors"]).OrderBy(x => x[assessorsField]));
                            assessmentsOrderedByAssessor.Add(assessmentOrdered);
                        }
                        assessmentsToOrder = assessmentsOrderedByAssessor.OrderBy(x => 1);
                        if (order == "asc")
                        {
                            assessmentsToOrder.ThenBy(x => ((DynamicModel[])x["assessors"])[0][assessorsField]);
                        }
                        else
                        {
                            assessmentsToOrder.ThenByDescending(x => ((DynamicModel[])x["assessors"])[0][assessorsField]);
                        }
                        assessmentsOrderedByAssessor = assessmentsToOrder.ToList();
                    }

                    return assessmentsOrderedByAssessor.OrderBy(x => 1);
                };
                if (sorting.Sort[0].StartsWith("assessors."))
                {
                    assessmentsFirstOrdered = orderByAssessor(sorting.Order[0], sorting.Sort[0], false, assessmentsUnorderedButCasted.OrderBy(a => 1));
                }
                Func<string, string, IEnumerable<DynamicModel>> assessmentsThenBy = (order, field) => (order == "asc") ? assessmentsFirstOrdered.ThenBy(x => x[field]) : assessmentsFirstOrdered.ThenByDescending(x => x[field]);
                IEnumerable<DynamicModel> assessments = (sorting.Sort.Length >= 2) ? null : assessmentsFirstOrdered;
                for (int i = 1; i < sorting.Order.Length; i++)
                {
                    int j = i;
                    if (sorting.Sort[i].StartsWith("assessors."))
                    {
                        assessments = orderByAssessor(sorting.Order[i], sorting.Sort[i], true, (IOrderedEnumerable<DynamicModel>)assessmentsFirstOrdered);
                    }
                    else
                    {
                        assessments = assessmentsThenBy(sorting.Order[j], sorting.Sort[j]);
                    }
                }
                return assessments;
            }
        }


        public async Task<DynamicModel> FetchAssessment(Guid id)
        {
            CloudTable table = await Connect("Assessments");
            var query = TableOperation.Retrieve<DynamicEntity>("", id.ToString());
            var assessment = await table.ExecuteAsync(query);

            if (assessment.Result == null)
            {
                return null;
            }
            var results = AssessmentMapper.ToModel(assessment.Result);

            return results;
        }

        public async Task<DynamicModel> FetchAssessor(string userName)
        {
            CloudTable table = await Connect("Assessors");
            var query = TableOperation.Retrieve<DynamicEntity>("", userName);
            var assessor = await table.ExecuteAsync(query);

            if (assessor.Result == null)
            {
                return null;
            }
            var results = AssessorMapper.ToModel(assessor.Result);

            return results;
        }

        public async Task<IEnumerable<DynamicModel>> FetchAssessors()
        {
            CloudTable table = await Connect("Assessors");
            var query = new TableQuery<DynamicEntity>();
            var assessorsToMap = await table.ExecuteQuerySegmentedAsync(query, null);
            var assessors = assessorsToMap.Select(a => AssessorMapper.ToModel(a));
            return assessors;
        }

        private async Task<CloudTable> Connect(string tableName)
        {
            var account = CloudStorageAccount.Parse(_settings.ConnectionString);
            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();

            return table;
        }

        public async Task<DynamicModel> PostAssessment(dynamic assessment)
        {
            CloudTable table = await Connect("Assessments");
            DynamicEntity assessmentEntity = AssessmentMapper.ToEntity(assessment);

            TableOperation insertOperation = TableOperation.Insert(assessmentEntity);
            var tableResult = await table.ExecuteAsync(insertOperation);

            var result = AssessmentMapper.ToModel(tableResult.Result);
            return result;
        }

        public async Task<DynamicModel> PostAssessor(dynamic assessor)
        {
            CloudTable table = await Connect("Assessors");
            DynamicEntity assessorEntity = AssessorMapper.ToEntity(assessor);

            TableOperation insertOperation = TableOperation.InsertOrReplace(assessorEntity);
            var tableResult = await table.ExecuteAsync(insertOperation);

            var result = AssessorMapper.ToModel(tableResult.Result);
            return result;
        }

        public async Task PatchAssessment(DynamicModel assessment)
        {
            CloudTable table = await Connect("Assessments");
            var unmappedAssessment = AssessmentMapper.ToEntity(assessment);
            var query = TableOperation.InsertOrReplace(unmappedAssessment);
            await table.ExecuteAsync(query);
        }

        public async Task DeleteAssessments()
        {
            CloudTable table = await Connect("Assessments");
            await table.DeleteAsync();
        }

        private TableStorageSettings _settings;
    }
}