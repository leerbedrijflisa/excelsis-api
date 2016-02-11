using Microsoft.AspNet.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using Excelsis.Api;

namespace Excelsis.Api
{
    [Route("/assessments/")]
    public class AssessmentsController
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var assessments = await FetchAssessments();
            //var result = ConvertAssessments(assessments);
            
            return new HttpOkObjectResult(assessments);
        }

        private async Task<IEnumerable<object>> FetchAssessments()
        {
            var account = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference("assessments");
            await table.CreateIfNotExistsAsync();

            var query = new TableQuery<DynamicEntity>();
            var assessments = table.ExecuteQuerySegmentedAsync(query, null);

            return assessments.Result;
        }
    }
}