using System;
using System.Threading.Tasks;
using Lisa.Common.WebApi;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;

namespace Lisa.Excelsis.Api
{
    [Route("assessments")]
    public class AssessmentController : Controller
    {
        public AssessmentController(Database database)
        {
            _db = database;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string studentName, [FromQuery] string assessors, [FromQuery] string exam)
        {
            var assessments = await _db.FetchAssessments();
            var filter = new List<Filter>();

            if (!string.IsNullOrWhiteSpace(studentName))
            {
                filter.Add(new Filter("StudentName", studentName));
            }
            if (!string.IsNullOrWhiteSpace(assessors))
            {
                filter.Add(new Filter("Assessors", assessors));
            }
            if (!string.IsNullOrWhiteSpace(exam))
            {
                filter.Add(new Filter("Exam", exam));
            }
            if (filter.Count > 0)
            {
                assessments = FilterHandler.filterAssessments(assessments, filter);
            }

            return new HttpOkObjectResult(assessments);
        }

        [HttpGet("{id}", Name = "getSingle")]
        public async Task<ActionResult> Get(Guid id)
        {
            var result = await _db.FetchAssessment(id);

            if (result == null)
            {
                return new HttpNotFoundResult();
            }

            return new HttpOkObjectResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] DynamicModel assessment)
        {
            if ( assessment == null )
            {
                return new BadRequestResult();
            }

            var validationResult = new AssessmentValidator().Validate(assessment);
            if (validationResult.HasErrors)
            {
                return new UnprocessableEntityObjectResult(validationResult.Errors);
            }
            dynamic result = await _db.PostAssessment(assessment);
            var location = Url.RouteUrl("getSingle", new { Id = result.Id }, Request.Scheme);

            return new CreatedResult(location, result);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(Guid id, [FromBody] Patch[] patches)
        {
            if (patches == null)
            {
                return new BadRequestResult();
            }

            var assessment = await _db.FetchAssessment(id);

            if (assessment == null)
            {
                return new HttpNotFoundResult();
            }
            var validationResult = new AssessmentValidator().Validate(patches, assessment);
            if (validationResult.HasErrors)
            {
                return new UnprocessableEntityObjectResult(validationResult.Errors);
            }

            var patcher = new ModelPatcher();

            patcher.Apply(patches, assessment);

            await _db.PatchAssessment(assessment);
            return new HttpOkObjectResult(assessment);
        }

        private Database _db;
    }
}
