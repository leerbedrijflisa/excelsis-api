using Microsoft.AspNet.Mvc;
using System;
using System.Threading.Tasks;

namespace Lisa.Excelsis.Api
{
    [Route("Assessments")]
    public class AssessmentController
    {
        public AssessmentController(Database database)
        {
            _db = database;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _db.FetchAssessments(id);

            return new HttpOkObjectResult(result);
        }
        private Database _db;
    }
}