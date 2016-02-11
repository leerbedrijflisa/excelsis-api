using Microsoft.AspNet.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Excelsis.Api
{
    [Route("/assessments/")]
    public class Assessments
    {
        [HttpGet]
        public IActionResult Get()
        {
            //var assessments = await FetchAssessments();

            return new HttpOkObjectResult("ok");
        }

        //private async Task<IEnumerable<object>> FetchAssessments()
        //{
        //    var account = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
        //    var client = account.CreateCloudTableClient();
        //    var table = client.GetTableReference("assessments");
        //    await table.CreateIfNotExistsAsync();

        //    var query = new TableQuery<DynamicEntity>();
        //    var assessments = table.ExecuteQuerySegmentedAsync(query, null);

        //    return assessments.Result;
        //}
    }
}