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
            var filterValues = new List<Filter>();

            foreach (Filter filter in filters)
            {
                if (filter.Type == "StudentName")
                {
                    if (filter.Value.Contains(","))
                    {
                        var studentNames = filter.Value.Split(',');
                        filterValues.Add(new Filter("StudentName", studentNames.ToString()));
                    }
                    else
                    {
                        filterValues.Add(new Filter("StudentName", filter.Value));
                    }
                }

                if (filter.Type == "Assessors")
                {
                    if (filter.Value.Contains(","))
                    {
                        string[] assessorNames = filter.Value.Split(',');
                        filterValues.Add(new Filter("Assessors,OR", assessorNames.ToString()));
                    }
                    else if (filter.Value.Contains(";"))
                    {
                        string[] assessorNames = filter.Value.Split(';');
                        filterValues.Add(new Filter("Assessors,AND", assessorNames.ToString()));
                    }
                    else
                    {
                        filterValues.Add(new Filter("Assessors,SINGLE", filter.Value));
                    }
                }
            }

            foreach (dynamic assessment in assessments)
            {
                string k = "m";
            }
            
            return result;
        }
    }
}