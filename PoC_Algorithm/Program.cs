// See https://aka.ms/new-console-template for more information
using PoC_Algorithm;

Console.WriteLine("Hello, World!");

var generator = new AssignmentGenerator();


DateTime routeStartTime = DateTime.Parse("2023-08-18 08:00");
DateTime routeEndTime = DateTime.Parse("2023-08-18 16:00");
var assignmentsGenerated = generator.GenerateAssignments(150, routeStartTime, routeEndTime, 1800);
var distancesGenerated = generator.GenerateDistances(assignmentsGenerated, 5400);

Console.WriteLine("\n ****VERSION 2***** \n");

RoutePlannerOriginal planner2 = new RoutePlannerOriginal();
var optimalRoutes2 = planner2.FindOptimalRoutes(assignmentsGenerated, distancesGenerated, routeStartTime, routeEndTime);

int linecounter = 1;
for (int i = 0; i < optimalRoutes2.Count; i++)
{
    var currentRoute = optimalRoutes2[i].OrderBy( x => x.ArrivalTime );

    foreach (var asignemt in currentRoute)
    {
        Console.WriteLine($"Linenumber {linecounter} - Route {i}: " + asignemt.Name + " Ariavel time is: " + asignemt.ArrivalTime);
        linecounter++;
    }
}

Console.WriteLine("Progam Terminated");
Console.ReadKey();






