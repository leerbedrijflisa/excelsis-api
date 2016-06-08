using System.Linq;

namespace Lisa.Excelsis.Api
{
    public class Sorting
    {
        public Sorting(string[] sort, string[] order)
        {
            Sort = sort;
            Order = order;
        }

        public string[] Sort
        {
            get
            {
                return _sort;
            }
            private set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    value = value.Where(val => val == "student.name" || val == "student.number" || val == "crebo" || val == "cohort" || val == "subject" || val == "exam" || val == "assessors.firstName" || val == "assessors.lastName" || val == "assessors.userName" || val == "teacherCode" || val == "assessed").ToArray();
                }
                _sort = value;
            }
        }

        public string[] Order
        {
            get
            {
                return _order;
            }
            private set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    value = value.Where(val => val == "asc" || val == "desc").ToArray();
                }
                _order = value;
            }
        }

        private string[] _sort;
        private string[] _order;
    }
}