using Lisa.Common.WebApi;
using Newtonsoft.Json;
using System.Collections.Generic;

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
                        filterValues["StudentName"] = new Filter("OR", studentNames);
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
                        var k = filter.Value.Split(';');
                        string assessorNames = JsonConvert.SerializeObject(filter.Value.Split(';'));
                        filterValues["Assessors"] = new Filter("AND", assessorNames);
                    }
                    else
                    {
                        filterValues["Assessors"] = new Filter("SINGLE", filter.Value);
                    }
                }

                if (filter.Type == "Exam")
                {
                    filterValues["Exam"] = new Filter("SINGLE", filter.Value);
                }
            }

            foreach (dynamic assessment in assessments)
            {
                int addToResult = 0;
                if (filterValues.ContainsKey("StudentName"))
                {
                    if (StudentNameCheck(filterValues["StudentName"], assessment))
                    {
                        addToResult = 1;
                    }
                    else
                    {
                        addToResult = 2;
                    }
                }

                if (filterValues.ContainsKey("Assessors") && addToResult != 2)
                {
                    if (AssessorsCheck(filterValues["Assessors"], assessment))
                    {
                        addToResult = 1;
                    }
                    else
                    {
                        addToResult = 2;
                    }
                }

                if (filterValues.ContainsKey("Exam") && addToResult != 2)
                {
                    if (assessment.ExamSubject.ToLower().Contains(filterValues["Exam"].Value.ToLower()) || assessment.ExamName.ToLower().Contains(filterValues["Exam"].Value.ToLower()))
                    {
                        addToResult = 1;
                    }
                    else
                    {
                        addToResult = 2;
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
            if (studentName.Type == "OR")
            {
                var studentNamesArray = JsonConvert.DeserializeObject<List<string>>(studentName.Value);
                foreach (var item in studentNamesArray)
                {
                    if (assessment.StudentName.ToLower() == item.ToLower())
                    {
                        return true;
                    }
                }
            }
            else if (studentName.Type == "SINGLE")
            {
                if (assessment.StudentName.ToLower() == studentName.Value.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
    }
}