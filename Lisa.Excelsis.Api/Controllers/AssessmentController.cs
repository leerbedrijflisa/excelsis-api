using Lisa.Common.WebApi;
using Microsoft.AspNet.Mvc;
using System;
using System.Threading.Tasks;

namespace Lisa.Excelsis.Api
{
    [Route("assessments")]
    public class AssessmentController
    {
        public AssessmentController(Database database)
        {
            _db = database;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _db.FetchAssessment(id);

            if (result == null)
            {
                return new HttpNotFoundResult();
            }

            return new HttpOkObjectResult(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] Patch[] patches)
        {
            var assessment = await _db.FetchAssessment(id);

            if (assessment == null)
            {
                return new HttpNotFoundResult();
            }

            var patcher = new ModelPatcher();

            patcher.Apply(patches, assessment);

            await _db.PatchAssessment(assessment);
            return new HttpOkObjectResult(assessment);
        }

        private Database _db;
    }
}