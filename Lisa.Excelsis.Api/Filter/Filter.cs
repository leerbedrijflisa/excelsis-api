using Lisa.Common.WebApi;
using System;
using System.Collections.Generic;

namespace Lisa.Excelsis.Api
{
    public class Filter
    {
        public static IEnumerable<DynamicModel> Main()
        {
            var users = new List<DynamicModel>();
            dynamic user = new DynamicModel();
            user.FirstName = "Ramon";
            user.LastName = "Meijers";
            user.Age = 19;
            user.Hobbies = new string[] { "games", "movies" };
            users.Add(user);
            user = new DynamicModel();
            user.FirstName = "Marvin";
            user.LastName = "Meijwaard";
            user.Hobbies = new string[] { "games"};
            user.Age = 24;
            users.Add(user);
            user = new DynamicModel();
            user.FirstName = "Roald";
            user.LastName = "Teunissen";
            user.Age = 19;
            users.Add(user);

            var filter = new List<FilterProperties>();
            filter.Add(new CompositeOrFilter(new string[] { "Hobbies", "Meeeeeep" }, new string[] { "movies", "games" }));
            filter.Add(new AndFilter("Hobbies", new string[] { "movies" }));
            filter.Add(new OrFilter( "FirstName", new string[] { "Marvin", "Ramon" }));


            var result = UseFilter(users, filter);

            foreach (dynamic item in result)
            {
                Console.WriteLine(item.FirstName + " " + item.LastName); 
            }
            return null;
        }

        public static IEnumerable<dynamic> UseFilter(IEnumerable<DynamicModel> unproccesedList, IEnumerable<FilterProperties> filters)
        {
            foreach (var unprocessedItem in unproccesedList)
            {
                if (ApplyFilters(unprocessedItem, filters))
                {
                    yield return unprocessedItem;
                }
            }
        }

        private static bool ApplyFilters(dynamic unprocessedItem, IEnumerable<FilterProperties> filters)
        {
            foreach (var filter in filters)
            {
                if (!filter.Apply(unprocessedItem))
                {
                    return false;
                }
            }
            return true;
        }
    }
}