using Lisa.Common.WebApi;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lisa.Excelsis.Api
{
    [Route("assessors")]
    public class AssessorController : Controller
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

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] DynamicModel assessor)
        {
            if (assessor == null)
            {
                return new BadRequestResult();
            }

            dynamic result = await _db.PostAssessor(assessor);
            var location = "";

            return new CreatedResult(location, result);
        }

        private Database _db;
    }
}