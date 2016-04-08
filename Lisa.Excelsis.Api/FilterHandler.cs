using Lisa.Common.WebApi;
using System.Collections.Generic;

namespace Lisa.Excelsis.Api
{
    public class FilterHandler
    {
        public static IEnumerable<DynamicModel> filterAssessments(IEnumerable<DynamicModel> assessments, IEnumerable<Filter> filters)
        {
            var result = new List<DynamicModel>();
            foreach (dynamic assessment in assessments)
            {
                foreach (Filter filter in filters)
                {
                    if (filter.Type == "StudentName")
                    {

                        if (filter.Value.Contains("+") )
                        {
                            return null;
                        }

                        if (filter.Value.Contains(","))
                        {
                            var studentNames = filter.Value.Split(',');
                            foreach (var studentName in studentNames)
                            {
                                if (studentName.ToLower() == assessment.StudentName.ToLower())
                                {
                                    result.Add(assessment);
                                    break;
                                }
                            }
                        }

                        //you should be to filter on multiple students
                        if (assessment.StudentName.ToLower() == filter.Value.ToLower())
                        {
                            result.Add(assessment);
                            break;
                        }
                    }
                    if (filter.Type == "Assessors")
                    {
                        if (filter.Value.Contains(","))
                        {
                            var assessorNames = filter.Value.Split(',');
                            foreach (var assessorName in assessorNames)
                            {
                                if (assessment.Assessors.Contains(assessorName))
                                {
                                    result.Add(assessment);
                                    break;
                                }
                            }
                        }

                        if (filter.Value.Contains(";"))
                        {
                            string[] assessorNames = filter.Value.Split(';');
                            if (assessment.Assessors.Any(assessorNames.Contains))
                            {
                                result.Add(assessment);
                                break;
                            }
                        }
                        
                        if (assessment.Assessors.ToLower().Contains(filter.Value.ToLower()))
                        {

                            result.Add(assessment);
                            break;
                        }
                    }
                }
            }
            
            return result;
        }
    }
}