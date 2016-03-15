using Lisa.Common.TableStorage;
using Lisa.Common.WebApi;
using Microsoft.Extensions.OptionsModel;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace Lisa.Excelsis.Api
{
    public class Database
    {
        public Database(IOptions<TableStorageSettings> settings)
        {
            _settings = settings.Value;
        }

        private async Task<CloudTable> Connect()
        {
            var account = CloudStorageAccount.Parse(_settings.ConnectionString);
            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference("Assessments");
            await table.CreateIfNotExistsAsync();

            return table;
        }

        public async Task<DynamicModel> PostAssessment(dynamic assessment)
        {
            CloudTable table = await Connect();
            DynamicEntity assessmentEntity = AssessmentMapper.ToEntity(assessment);

            TableOperation insertOperation = TableOperation.Insert(assessmentEntity);
            var tableResult = await table.ExecuteAsync(insertOperation);

            var result = AssessmentMapper.ToModel(tableResult.Result);
            return result;
        }

        private TableStorageSettings _settings;
    }
}