using Lisa.Common.WebApi;
using Microsoft.AspNet.Mvc;
using System;
using System.Threading.Tasks;

namespace Lisa.Excelsis.Api
{
    [Route("/assessments/")]
    public class AssessmentController : Controller
    {
        public AssessmentController(Database database)
        {
            _db = database;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] DynamicModel assessment)
        {
            dynamic result = await _db.PostAssessment(assessment);
            var location = Url.RouteUrl("getSingle", new { Id = result.Id }, Request.Scheme);

            return new CreatedResult(location, result);
        }

        [HttpGet]
        [Route("{id}", Name = "getSingle")]
        public async Task<ObjectResult> Get(Guid id)
        {
            var result = new
            {
                Id = Guid.NewGuid(),
                Observations = new[] { "seen", "not seen", "not rated" }
            };
            return new HttpOkObjectResult(result);
        }

        private Database _db;
    }
}
