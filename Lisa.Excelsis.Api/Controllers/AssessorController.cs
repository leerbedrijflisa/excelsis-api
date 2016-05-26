using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return new OkObjectResult(result);
        }

        private Database _db;
    }
}