using Lisa.Common.WebApi;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Lisa.Excelsis.Api
{
    public class FilterHandler
    {
        public static IEnumerable<DynamicModel> filterAssessments(IEnumerable<DynamicModel> assessments, IEnumerable<Filter> filters)
        {
            var result = new List<DynamicModel>();
            var filterValues = new Dictionary<string, Filter>();

            foreach (Filter filter in filters)
            {
                if (filter.Type == "StudentName")
                {
                    if (filter.Value.Contains(","))
                    {
                        string studentNames = JsonConvert.SerializeObject(filter.Value.Split(','));
                        filterValues["StudentName"] = new Filter("ARRAY", studentNames);
                    }
                    else
                    {
                        filterValues["StudentName"] = new Filter("SINGLE", filter.Value);
                    }
                }

                if (filter.Type == "Assessors")
                {
                    if (filter.Value.Contains(","))
                    {
                        string assessorNames = JsonConvert.SerializeObject(filter.Value.Split(','));
                        filterValues["Assessors"] = new Filter("OR", assessorNames);
                    }
                    else if (filter.Value.Contains(";"))
                    {
                        string assessorNames = JsonConvert.SerializeObject(filter.Value.Split(';'));
                        filterValues["Assessors"] = new Filter("AND", assessorNames);
                    }
                    else
                    {
                        filterValues["Assessors"] = new Filter("SINGLE", filter.Value);
                    }
                }
            }

            foreach (dynamic assessment in assessments)
            {
                int addToResult = 0;
                if (filterValues.ContainsKey("StudentName"))
                {
                    //if studentname == filtervalue studentname add to result
                }

                if (filterValues.ContainsKey("Assessors") && addToResult != 2)
                {
                    addToResult = 0;
                    if (AssessorsCheck(filterValues["Assessors"], assessment))
                    {
                        addToResult = 1;
                    }
                }

                if (addToResult == 1)
                {
                    result.Add(assessment);
                }

            }
            
            return result;
        }

        public static bool AssessorsCheck(Filter assessors, dynamic assessment)
        {
            var caseInsensitive = JsonConvert.DeserializeObject<List<string>>(JsonConvert.SerializeObject(assessment.Assessors).ToLower());
            if (assessors.Type == "OR")
            {
                var assessorsArray = JsonConvert.DeserializeObject<List<string>>(assessors.Value);
                foreach (var item in assessorsArray)
                {
                    if (caseInsensitive.Contains(item.ToLower()))
                    {
                        return true;
                    }
                }
            }
            else if (assessors.Type == "AND")
            {
                var assessorsArray = JsonConvert.DeserializeObject<List<string>>(assessors.Value);
                bool containsAll = true;
                foreach (var item in assessorsArray)
                {
                    if (!caseInsensitive.Contains(item.ToLower()))
                    {
                        containsAll = false;
                    }
                }
                if (containsAll)
                {
                    return true;
                }
            }
            else if (assessors.Type == "SINGLE")
            {
                if (caseInsensitive.Contains(assessors.Value.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool StudentNameCheck(Filter studentName, dynamic assessment)
        {
            if (studentName.Type == "ARRAY")
            {
                
            }
            return false;
        }
    }
}