namespace PoC_Algorithm
{
    public class RoutePlannerReFactored
    {
        public List<List<Assignment>> FindOptimalRoutes(List<Assignment> allAssignments, List<DistanceTuple> distances, DateTime routeStartTime, DateTime routeEndTime)
        {
            List<Assignment> availableAssignments = new List<Assignment>(allAssignments.Where(a => a.TimeWindowStart <= routeEndTime && a.TimeWindowEnd >= routeStartTime)).OrderBy(a => a.TimeWindowStart).ToList();
            List<List<Assignment>> plannedRoutes = new List<List<Assignment>>();

            while (availableAssignments.Any())
            {
                DateTime routeTimeTracker = routeStartTime;
                Assignment startAssignment = availableAssignments.First();
                SetArriavalTimeForStartAssignment(startAssignment);
                routeTimeTracker = NewMethod(routeTimeTracker, startAssignment);
                Assignment currentAssignment = SetStartAssignmentAsCurrent(startAssignment);
                List<Assignment> assignmentsToRemove = MakeNewAssignmentsToRemoce();
                List<Assignment> route = MakeNewRoute(startAssignment);

                while (SoLongThereIsStillAssignmentsToCalculate(currentAssignment))
                {
                    Assignment nextAssignment = ResetNextAssignment();
                    double shortestDistanceToPotentialNext = SetDistanceToMaxForCalculation();

                    foreach (var potentialNextAssignment in availableAssignments.Where(a => a != currentAssignment && a.TimeWindowEnd >= routeTimeTracker && route.Contains(a) == false))
                    {
                        DistanceTuple? distanceToFromCurrentToPotentialNextTuple = GetDistance(distances, currentAssignment, potentialNextAssignment);

                        if (IsNoDistanceFound(distanceToFromCurrentToPotentialNextTuple))
                        {
                            // implement use of api to get distance
                            // set new distance to distanceToFromCurrentToPotentialNextTuple
                        }

                        int travelTimeFromCurrentToPotentialNext = GetDistance(distanceToFromCurrentToPotentialNextTuple);
                        if (IsTheDistanceShorterThenCurrentNextAssigment(shortestDistanceToPotentialNext, travelTimeFromCurrentToPotentialNext))
                        {
                            shortestDistanceToPotentialNext = SetNewShortestDistance(travelTimeFromCurrentToPotentialNext);
                            nextAssignment = SetNewNextAssignment(potentialNextAssignment);
                        }

                    }

                    if (IsNextAssignmentFound(nextAssignment))
                    {
                        routeTimeTracker = AddNewTimeInformation(routeTimeTracker, nextAssignment, shortestDistanceToPotentialNext);
                        currentAssignment = AddNewRaouteInformation(assignmentsToRemove, route, nextAssignment);
                    }
                    else
                    {
                        currentAssignment = null;
                    }
                }

                plannedRoutes.Add(route);
                RemoveUsedAssigments(availableAssignments, startAssignment, assignmentsToRemove);

            }

            return plannedRoutes;
        }

        private static void SetArriavalTimeForStartAssignment(Assignment startAssignment)
        {
            startAssignment.ArrivalTime = startAssignment.TimeWindowStart;
        }

        private static DateTime NewMethod(DateTime routeTimeTracker, Assignment startAssignment)
        {
            routeTimeTracker = routeTimeTracker.AddSeconds(startAssignment.Duration);
            return routeTimeTracker;
        }

        private static Assignment SetStartAssignmentAsCurrent(Assignment startAssignment)
        {
            return startAssignment;
        }

        private static List<Assignment> MakeNewAssignmentsToRemoce()
        {
            return new List<Assignment>();
        }

        private static List<Assignment> MakeNewRoute(Assignment startAssignment)
        {
            return new List<Assignment> { startAssignment };
        }

        private static bool SoLongThereIsStillAssignmentsToCalculate(Assignment currentAssignment)
        {
            return currentAssignment != null;
        }

        private Assignment ResetNextAssignment()
        {
            return null;
        }

        private double SetDistanceToMaxForCalculation()
        {
            return Double.MaxValue;
        }

        private  DistanceTuple? GetDistance(List<DistanceTuple> distances, Assignment currentAssignment, Assignment? potentialNextAssignment)
        {
            return distances.FirstOrDefault(d =>
                (d.Assignment1 == currentAssignment && d.Assignment2 == potentialNextAssignment) ||
                (d.Assignment2 == currentAssignment && d.Assignment1 == potentialNextAssignment));
        }

        private bool IsNoDistanceFound(DistanceTuple? distanceToFromCurrentToPotentialNextTuple)
        {
            return distanceToFromCurrentToPotentialNextTuple == null;
        }

        private int GetDistance(DistanceTuple? distanceToFromCurrentToPotentialNextTuple)
        {
            return distanceToFromCurrentToPotentialNextTuple.Distance;
        }

        private bool IsTheDistanceShorterThenCurrentNextAssigment(double shortestDistanceToPotentialNext, int travelTimeFromCurrentToPotentialNext)
        {
            return travelTimeFromCurrentToPotentialNext < shortestDistanceToPotentialNext;
        }

        private int SetNewShortestDistance(int travelTimeFromCurrentToPotentialNext)
        {
            return travelTimeFromCurrentToPotentialNext;
        }

        private Assignment SetNewNextAssignment(Assignment? potentialNextAssignment)
        {
            return potentialNextAssignment;
        }

        private bool IsNextAssignmentFound(Assignment nextAssignment)
        {
            return nextAssignment != null;
        }

        private DateTime AddNewTimeInformation(DateTime routeTimeTracker, Assignment nextAssignment, double shortestDistanceToPotentialNext)
        {
            routeTimeTracker = routeTimeTracker.AddSeconds(shortestDistanceToPotentialNext);
            nextAssignment.ArrivalTime = routeTimeTracker;
            routeTimeTracker = routeTimeTracker.AddSeconds(nextAssignment.Duration);
            return routeTimeTracker;
        }

        private Assignment AddNewRaouteInformation(List<Assignment> assignmentsToRemove, List<Assignment> route, Assignment nextAssignment)
        {
            Assignment currentAssignment;
            route.Add(nextAssignment);
            assignmentsToRemove.Add(nextAssignment);
            currentAssignment = nextAssignment;
            return currentAssignment;
        }

        private void RemoveUsedAssigments(List<Assignment> availableAssignments, Assignment startAssignment, List<Assignment> assignmentsToRemove)
        {
            availableAssignments.Remove(startAssignment);
            foreach (var assignmentToRemove in assignmentsToRemove)
            {
                availableAssignments.Remove(assignmentToRemove);
            }
        }
    }
}
