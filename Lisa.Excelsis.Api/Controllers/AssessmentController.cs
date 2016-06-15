using System;
using System.Threading.Tasks;
using Lisa.Common.WebApi;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Primitives;

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
        public async Task<ActionResult> Get([FromQuery] string studentName, [FromQuery] string studentNumber)
        {
            var assessments = await _db.FetchAssessments();
           // assessments = applyFilter(assessments);
            //var assessorsKey = Request.Query.Keys.Where(k => k.Contains("assessors"));
            //if (assessorsKey.Count() != 0)
            //{
            //    foreach (dynamic assessment in assessments)
            //    {
            //        foreach (var assessors in assessment.Assessors)
            //        {
            //            var assessorsKeys = new List<string>();
            //            var assessorsValues = new List<string[]>();
            //            foreach (var item in assessorsKey)
            //            {
            //                StringValues values;
            //                Request.Query.TryGetValue(item, out values);
            //                assessorsValues.Add(values.ToArray());
            //                assessorsKeys.Add(item.Split('.')[1]);
            //            }

            //            var assessorsKeyArray = assessorsKeys.ToArray();
            //            var filterAssessors = new List<FilterProperties>();

            //            for (int i = 0; i < assessorsKeyArray.Count(); i++)
            //            {
            //                filterAssessors.Add(new OrFilter(assessorsKeyArray[i], assessorsValues[i]));
            //            }
            //            var r = new List<DynamicModel>();
            //            foreach (var assessorsItem in assessors)
            //            {
            //                r.Add(assessorsItem);
            //            }
            //        }
            //    }
            //}

            var derpiederp = new OrFilter(
                                new AndFilter(
                                    new EqualsFilter("assessors.FirstName", "Peter"),
                                    new EqualsFilter("assessors.LastName", "Snoek")
                                )
                            );
            assessments = derpiederp.Apply(assessments);

            return new OkObjectResult(assessments);
        }

        [HttpGet("{id}", Name = "getSingle")]
        public async Task<ActionResult> Get(Guid id)
        {
            var result = await _db.FetchAssessment(id);

            if (result == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(result);
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
                return new NotFoundResult();
            }
            var validationResult = new AssessmentValidator().Validate(patches, assessment);
            if (validationResult.HasErrors)
            {
                return new UnprocessableEntityObjectResult(validationResult.Errors);
            }

            var patcher = new ModelPatcher();

            patcher.Apply(patches, assessment);

            await _db.PatchAssessment(assessment);
            return new OkObjectResult(assessment);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete()
        {
            await _db.DeleteAssessments();
            return new StatusCodeResult(204);
        }

        private static string[] allowedFields = new string[] {
            "student.name",
            "student.number",
            "exam",
            "subject",
            "assessors.firstname",
            "assessors.lastname",
            "assessors.username",
            "assessors.teachercode"
        };

        //private IEnumerable<DynamicModel> applyFilter(IEnumerable<DynamicModel> assessments) {
        //    List<FilterProperties> filters = new List<FilterProperties>();
        //    foreach (var Key in Request.Query.Keys)
        //    {
        //        var value = (string)Request.Query[Key];
        //        if (allowedFields.Contains(Key.ToLower()) && value != string.Empty)
        //        {
        //            string[] filterStudentNames;
        //            if (value.Contains(','))
        //            {
        //                filterStudentNames = value.Split(',');
        //            }
        //            else
        //            {
        //                filterStudentNames = new string[] { value };
        //            }
        //            filters.Add(new OrFilter(Key, filterStudentNames));
        //        }
        //    }
        //    //filters.Add(new CompositeAndFilter(new string[] { "assessors.firstname", "assessors.lastname" }, new string[] { "joost", "ronkes agerbeek" }));
        //    return Filter.UseFilter(assessments, filters);
        //}

        private Database _db;
    }
}