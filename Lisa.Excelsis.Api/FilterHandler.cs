using Lisa.Common.WebApi;
using System.Collections.Generic;
using System.Linq;

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
                        //you are should be to filter on multiple students
                        if (assessment.StudentName == filter.Value)
                        {
                            result.Add(assessment);
                            break;
                        }
                    }
                    if (filter.Type == "Assessors")
                    {
                        //you should be able to filter on multiple assessors

                    }
                }
            }
            
            return null;
        }
    }
}