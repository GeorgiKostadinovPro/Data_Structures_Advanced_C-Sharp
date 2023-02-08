using System;
using System.Collections.Generic;

namespace Exam.MoovIt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var firstRoute = new Route("1", 3, 5, true, new List<string>() { "Sofia", "Plovdiv", "Varna" });
            var secondRoute = new Route("2", 3, 3, false, new List<string>() { "Sofia", "Pazardzhik", "Varna" });

            var routes = new HashSet<Route>();
            routes.Add(firstRoute);
            routes.Add(secondRoute);

            Console.WriteLine(routes.Count);
        }
    }
}