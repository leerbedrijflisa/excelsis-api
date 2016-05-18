using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace Lisa.Excelsis.Api
{
    [Route("assessors")]
    public class AssessorController
    {
        public AssessorController(Database database)
        {
            _db = database;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await _db.FetchAssessors();
            return new HttpOkObjectResult(result);
        }

        private Database _db;
    }
}