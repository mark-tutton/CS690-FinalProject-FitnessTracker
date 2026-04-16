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

public class SessionStats
{
    public DateTime SessionDate { get; }
    public string RoutineName { get; }
    public int TotalSets { get; }
    public int TotalReps { get; }
    public double TotalMinutes { get; }
    public double TotalDistance { get; }

    public SessionStats(
        string routineName,
        DateTime sessionDate,
        int totalSets,
        int totalReps,
        double totalMinutes,
        double totalDistance
    )
    {
        RoutineName = routineName;
        SessionDate = sessionDate;
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

    // compare 2 most recent sessions for a given exercise type
    public List<WorkoutSession> GetTwoMostRecentByType(
        List<WorkoutSession> sessions,
        ExerciseType type
    )
    {
        // find all sessions that contain at least one exercise of the given type
        var matchingSessions = new List<WorkoutSession>();
        foreach (var session in sessions)
        {
            foreach (var exercise in session.Routine.Exercises)
            {
                if (exercise != null && exercise.ExType == type)
                {
                    matchingSessions.Add(session);
                    break;
                }
            }
        }

        // newest to oldest
        matchingSessions.Sort((a, b) => b.SessionDate.CompareTo(a.SessionDate));

        // 2 most recent
        if (matchingSessions.Count >= 2)
        {
            return matchingSessions.GetRange(0, 2);
        }
        return matchingSessions;
    }

    public SessionStats ParseSessionStatsByType(WorkoutSession session, ExerciseType type)
    {
        // collect the names of exercises in this session that match the given type
        var exerciseNames = new List<string>();
        foreach (var exercise in session.Routine.Exercises)
        {
            if (exercise != null && exercise.ExType == type)
            {
                exerciseNames.Add(exercise.ExerciseName);
            }
        }

        int totalSets = 0;
        int totalReps = 0;
        double totalMinutes = 0;
        double totalDistance = 0;

        if (!string.IsNullOrEmpty(session.Notes))
        {
            // notes look like: "Bench Press: sets=3,reps=10; Running: miles=2,min=30"
            foreach (var entry in session.Notes.Split("; "))
            {
                var separatorIndex = entry.IndexOf(": ");
                if (separatorIndex >= 0)
                {
                    var exerciseName = entry.Substring(0, separatorIndex);
                    if (exerciseNames.Contains(exerciseName))
                    {
                        // parse the key=value pairs after the exercise name
                        var statsPart = entry.Substring(separatorIndex + 2);
                        foreach (var keyValuePair in statsPart.Split(","))
                        {
                            var keyValue = keyValuePair.Split("=");
                            if (keyValue.Length == 2 && double.TryParse(keyValue[1], out double val))
                            {
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
                }
            }
        }

        return new SessionStats(
            session.Routine.WorkoutRoutineName,
            session.SessionDate,
            totalSets,
            totalReps,
            totalMinutes,
            totalDistance
        );
    }
}
