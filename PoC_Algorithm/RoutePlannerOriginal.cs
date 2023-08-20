namespace PoC_Algorithm
{
    /// <summary>
    /// This is the original first version that actually worked as intended, DO NOT EDIT!
    /// This should be used as the Prof of Concept on how we think it should work, made
    /// with inspiration from Edger Dijkstra's shortest path trough a graph algorithm
    /// </summary>
    public class RoutePlannerOriginal
    {
        public List<List<Assignment>> FindOptimalRoutes(List<Assignment> allAssignments, List<DistanceTuple> distances, DateTime routeStartTime, DateTime routeEndTime)
        {
            List<Assignment> availableAssignments = new List<Assignment>(allAssignments.Where(a => a.TimeWindowStart <= routeEndTime && a.TimeWindowEnd >= routeStartTime)).OrderBy(a => a.TimeWindowStart).ToList();
            List<List<Assignment>> plannedRoutes = new List<List<Assignment>>();

            while (availableAssignments.Any())
            {
                DateTime routeTimeTracker = routeStartTime;
                Assignment startAssignment = availableAssignments.First();

                startAssignment.ArrivalTime = startAssignment.TimeWindowStart;
                routeTimeTracker = routeTimeTracker.AddSeconds(startAssignment.Duration);

                Assignment currentAssignment = startAssignment;

                List<Assignment> assignmentsToRemove = new List<Assignment>();
                List<Assignment> route = new List<Assignment> { startAssignment };

                while (currentAssignment != null)
                {
                    Assignment nextAssignment = null;
                    double shortestDistanceToPotentialNext = Double.MaxValue;

                    foreach (var potentialNextAssignment in availableAssignments.Where(a => a != currentAssignment && a.TimeWindowEnd >= routeTimeTracker && route.Contains(a) == false))
                    {
                        var distanceToFromCurrentToPotentialNextTuple = distances.FirstOrDefault(d =>
                            (d.Assignment1 == currentAssignment && d.Assignment2 == potentialNextAssignment) ||
                            (d.Assignment2 == currentAssignment && d.Assignment1 == potentialNextAssignment));

                        if (distanceToFromCurrentToPotentialNextTuple == null)
                        {
                            // implement use of api to get distance
                            // set new distance to distanceToFromCurrentToPotentialNextTuple
                        }

                        var travelTimeFromCurrentToPotentialNext = distanceToFromCurrentToPotentialNextTuple.Distance;
                        if (travelTimeFromCurrentToPotentialNext < shortestDistanceToPotentialNext)
                        {
                            shortestDistanceToPotentialNext = travelTimeFromCurrentToPotentialNext;
                            nextAssignment = potentialNextAssignment;
                        }

                    }

                    if (nextAssignment != null)
                    {
                        routeTimeTracker = routeTimeTracker.AddSeconds(shortestDistanceToPotentialNext);
                        nextAssignment.ArrivalTime = routeTimeTracker;

                        routeTimeTracker = routeTimeTracker.AddSeconds(nextAssignment.Duration);

                        route.Add(nextAssignment);
                        assignmentsToRemove.Add(nextAssignment);
                        currentAssignment = nextAssignment;
                    }
                    else
                    {
                        currentAssignment = null;
                    }
                }

                plannedRoutes.Add(route);
                availableAssignments.Remove(startAssignment);
                foreach (var assignmentToRemove in assignmentsToRemove)
                {
                    availableAssignments.Remove(assignmentToRemove);
                }

            }

            return plannedRoutes;
        }
    }
}
