namespace FitnessTracker;

public class RoutineStats
{
    public string RoutineName { get; }
    public int SessionCount { get; }
    public int TotalSets { get; }
    public int TotalReps { get; }
    public double TotalMinutes { get; }
    public double TotalDistance { get; }

    public RoutineStats(
        string routineName,
        int sessionCount,
        int totalSets,
        int totalReps,
        double totalMinutes,
        double totalDistance
    )
    {
        RoutineName = routineName;
        SessionCount = sessionCount;
        TotalSets = totalSets;
        TotalReps = totalReps;
        TotalMinutes = totalMinutes;
        TotalDistance = totalDistance;
    }
}

public class StatsManager
{
    public List<RoutineStats> GetStats(List<WorkoutSession> sessions)
    {
        //  group by routine name and aggreagte stats
        return sessions
            .GroupBy(s => s.Routine.WorkoutRoutineName)
            .Select(group => AggregateRoutineStats(group.Key, group.ToList()))
            .ToList();
    }

    private RoutineStats AggregateRoutineStats(string routineName, List<WorkoutSession> sessions)
    {
        int totalSets = 0;
        int totalReps = 0;
        double totalMinutes = 0;
        double totalDistance = 0;

        foreach (var session in sessions)
        {
            if (string.IsNullOrEmpty(session.Notes))
                continue;

            // notes are stored like: "Bench Press: sets=3,reps=10; Squats: sets=4,reps=12; Running: miles=2,min=30"
            foreach (var entry in session.Notes.Split("; "))
            {
                var separatorIndex = entry.IndexOf(": ");
                if (separatorIndex < 0)
                    continue;

                //ignore the ExerciseName and parse the key=value pairs after ": "
                foreach (var keyValuePair in entry[(separatorIndex + 2)..].Split(","))
                {
                    var keyValue = keyValuePair.Split("=");
                    if (keyValue.Length != 2 || !double.TryParse(keyValue[1], out double val))
                        continue;
                    switch (keyValue[0])
                    {
                        case "sets":
                            totalSets += (int)val;
                            break;
                        case "reps":
                            totalReps += (int)val;
                            break;
                        case "min":
                            totalMinutes += val;
                            break;
                        case "miles":
                            totalDistance += val;
                            break;
                    }
                }
            }
        }

        return new RoutineStats(
            routineName,
            sessions.Count,
            totalSets,
            totalReps,
            totalMinutes,
            totalDistance
        );
    }
}
