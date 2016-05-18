using System;
using System.Threading.Tasks;
using Lisa.Common.TableStorage;
using Lisa.Common.WebApi;
using Microsoft.Extensions.OptionsModel;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;

namespace Lisa.Excelsis.Api
{
    public class Database
    {
        public Database(IOptions<TableStorageSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<IEnumerable<DynamicModel>> FetchAssessments()
        {
            CloudTable table = await Connect("Assessments");
            var query = new TableQuery<DynamicEntity>();
            var assessmentsToMap = await table.ExecuteQuerySegmentedAsync(query, null);
            var assessments = assessmentsToMap.Select(a => AssessmentMapper.ToModel(a));
            return assessments;
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