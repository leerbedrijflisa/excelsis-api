using Lisa.Common.TableStorage;
using Lisa.Common.WebApi;
using Microsoft.Extensions.OptionsModel;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lisa.Excelsis.Api
{
    public class Database
    {
        public Database(IOptions<TableStorageSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<DynamicModel> FetchAssessment(Guid id)
        {
            CloudTable table = await Connect();
            var query = TableOperation.Retrieve<DynamicEntity>("", id.ToString());
            var assessment = await table.ExecuteAsync(query);

            if (assessment.Result == null)
            {
                return null;
            }
            var results = AssessmentMapper.ToModel(assessment.Result);

            return results;
        }
        
        public async Task PatchAssessment(DynamicModel assessment)
        {
            CloudTable table = await Connect();
            var unmappedAssessment = AssessmentMapper.ToEntity(assessment);
            var query = TableOperation.InsertOrReplace(unmappedAssessment);
            await table.ExecuteAsync(query);
        }

        private async Task<CloudTable> Connect()
        {
            var account = CloudStorageAccount.Parse(_settings.ConnectionString);
            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference("Assessments");
            await table.CreateIfNotExistsAsync();

            return table;
        }

        private TableStorageSettings _settings;
        private List<DynamicModel> _assessments = new List<DynamicModel>();
    }
}