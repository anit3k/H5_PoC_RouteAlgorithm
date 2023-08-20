namespace PoC_Algorithm
{
    public class AssignmentGenerator
    {
        private static Random random = new Random();

        public List<Assignment> GenerateAssignments(int count, DateTime startTime, DateTime endTime, int maxDuration)
        {
            List<Assignment> assignments = new List<Assignment>();

            double totalRouteTimeFrame = (endTime - startTime).TotalSeconds;

            // Divide the time period into three equal parts
            double timePeriod = totalRouteTimeFrame / 3;
            for (int i = 1; i <= count; i++)
            {
                double assignmentFrame = random.Next(1, 100);
                DateTime timeWindowStart;

                if (assignmentFrame > 1 && assignmentFrame < 33)
                {
                    timeWindowStart = startTime;
                }
                else if (assignmentFrame > 34 && assignmentFrame < 66)
                {
                    timeWindowStart = startTime.AddSeconds(timePeriod);
                }
                else
                {
                    timeWindowStart = startTime.AddSeconds(timePeriod + timePeriod);
                }

                DateTime timeWindowEnd = timeWindowStart.AddMinutes(180); // Adjust the duration as needed

                assignments.Add(new Assignment
                {
                    Id = i,
                    Name = $"Assignment {i}",
                    TimeWindowStart = timeWindowStart,
                    TimeWindowEnd = timeWindowEnd,
                    Duration = random.Next(1800, maxDuration),
                    ArrivalTime = DateTime.MinValue // Initialize ArrivalTime
                });
            }

            return assignments;
        }

        public List<DistanceTuple> GenerateDistances(List<Assignment> assignments, int maxDistance)
        {
            List<DistanceTuple> distances = new List<DistanceTuple>();

            for (int i = 0; i < assignments.Count; i++)
            {
                for (int j = i + 1; j < assignments.Count; j++)
                {
                    int distance = random.Next(1, maxDistance + 1);
                    distances.Add(new DistanceTuple
                    {
                        Assignment1 = assignments[i],
                        Assignment2 = assignments[j],
                        Distance = distance
                    });
                }
            }

            return distances;
        }
    }
}
