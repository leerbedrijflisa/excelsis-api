using System;
using System.Threading.Tasks;
using Lisa.Common.WebApi;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

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
        public async Task<ActionResult> Get()
        {
            var assessments = await _db.FetchAssessments();
            if (!string.IsNullOrWhiteSpace(Request.QueryString.ToString()))
            {
                var trimmedQueryString = Request.QueryString.ToString().TrimStart('?');
                if (!string.IsNullOrWhiteSpace(trimmedQueryString))
                {
                    var splittedQueryString = trimmedQueryString.Split('&');
                    var listValues = new List<Filter>();
                    var assessorValues = new List<KeyValuePair<string, string>>();
                    foreach (var queryItem in splittedQueryString)
                    {
                        var splittedQueryItem = queryItem.Split('=');
                        if (!allowedFields.Contains(splittedQueryItem[0].ToLower()))
                        {
                            continue;
                        }
                        if (splittedQueryString[0].Contains('.') && splittedQueryItem[0].Substring(0, splittedQueryItem[0].IndexOf('.')) == "assessors")
                        {
                            if (splittedQueryItem[1].Contains(","))
                            {
                                foreach (var item in splittedQueryItem[1].Split(','))
                                {
                                    assessorValues.Add(new KeyValuePair<string, string>(splittedQueryItem[0], item));
                                }
                            }
                        }
                        else
                        {
                            if (splittedQueryItem[1].Contains(","))
                            {
                                listValues.Add(new EqualsFilter(splittedQueryItem[0], splittedQueryItem[1].Split(',')));
                            }
                            else
                            {
                                listValues.Add(new EqualsFilter(splittedQueryItem[0], splittedQueryItem[1]));
                            }
                        }
                    }
                    var orFilter = new List<Filter>();
                    if (assessorValues.Count != 0)
                    {
                        var assessorLastName = assessorValues.Where(a => a.Key.ToLower() == "assessors.lastname").ToList();
                        var assessorFirstName = assessorValues.Where(a => a.Key.ToLower() == "assessors.firstname").ToList();
                        var assessorTeacherCode = assessorValues.Where(a => a.Key.ToLower() == "assessors.teachercode").ToList();
                        var assessorUserName = assessorValues.Where(a => a.Key.ToLower() == "assessors.username").ToList();
                        int highestArrayIndex = assessorLastName.Count();
                        if (highestArrayIndex < assessorFirstName.Count())
                        {
                            highestArrayIndex = assessorFirstName.Count();
                        }
                        if (highestArrayIndex < assessorTeacherCode.Count())
                        {
                            highestArrayIndex = assessorTeacherCode.Count();
                        }
                        if (highestArrayIndex < assessorUserName.Count())
                        {
                            highestArrayIndex = assessorUserName.Count();
                        }

                        for (int i = 0; i < highestArrayIndex; i++)
                        {
                            orFilter.Add(new AndFilter(
                                assessorLastName.Count() < i ? new EqualsFilter(assessorLastName[i].Key, assessorLastName[i].Value): null,
                                assessorFirstName.Count() < i ? new EqualsFilter(assessorFirstName[i].Key, assessorFirstName[i].Value) : null,
                                assessorTeacherCode.Count() < i ? new EqualsFilter(assessorTeacherCode[i].Key, assessorTeacherCode[i].Value) : null,
                                assessorUserName.Count() < i ? new EqualsFilter(assessorUserName[i].Key, assessorUserName[i].Value): null
                                ));
                        }
                        listValues.Add(new OrFilter(orFilter.ToArray()));
                    }
                    var assessmentsFilter = new AndFilter(
                                            listValues.ToArray()
                                     );
                    assessments = assessmentsFilter.Apply(assessments);
                }
            }
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