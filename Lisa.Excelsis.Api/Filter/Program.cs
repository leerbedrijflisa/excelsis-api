using Lisa.Common.WebApi;
using System;
using System.Collections.Generic;

namespace Lisa.Excelsis.Api
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    var users = FetchUsers();
        //    var filter = new OrFilter(
        //        new AndFilter(
        //            new EqualsFilter("Games.Name", "Overwatch"),
        //            new EqualsFilter("Games.Genre", "Shooter")
        //        ),
        //        new AndFilter(
        //            new EqualsFilter("Games.Name", "Rocket League"),
        //            new EqualsFilter("FirstName", "Ramon")
        //        )
        //    );

        //    var result = filter.Apply(users);

        //    foreach (dynamic user in result)
        //    {
        //        Console.WriteLine($"{user.FirstName} {user.LastName}");
        //    }
        //}

        private static IEnumerable<DynamicModel> FetchUsers()
        {
            // Games
            dynamic rocketLeague = new DynamicModel();
            rocketLeague.Name = "Rocket League";
            rocketLeague.Genre = "Sports";

            dynamic counterStrike = new DynamicModel();
            counterStrike.Name = "Counter-Strike";
            counterStrike.Genre = "Shooter";

            dynamic overwatch = new DynamicModel();
            overwatch.Name = "Overwatch";
            overwatch.Genre = "Shooter";

            // Addresses
            dynamic dordrecht = new DynamicModel();
            dordrecht.City = "Dordrecht";

            dynamic papendrecht = new DynamicModel();
            papendrecht.City = "Papendrecht";

            dynamic zwijndrecht = new DynamicModel();
            zwijndrecht.City = "Zwijndrecht";

            // Users
            dynamic user = new DynamicModel();
            user.FirstName = "Marvin";
            user.LastName = "Meijwaard";
            user.Games = new List<DynamicModel> { rocketLeague, counterStrike };
            user.Address = dordrecht;
            yield return user;

            user = new DynamicModel();
            user.FirstName = "Ramon";
            user.LastName = "Meijers";
            user.Games = new List<DynamicModel> { rocketLeague };
            user.Address = zwijndrecht;
            yield return user;

            user = new DynamicModel();
            user.FirstName = "Roald";
            user.LastName = "Teunissen";
            user.Games = new List<DynamicModel> { counterStrike, overwatch };
            user.Address = dordrecht;
            yield return user;

            user = new DynamicModel();
            user.FirstName = "Bart";
            user.LastName = "Schop";
            user.Games = new List<DynamicModel> { overwatch };
            user.Address = papendrecht;
            yield return user;
        }
    }
}