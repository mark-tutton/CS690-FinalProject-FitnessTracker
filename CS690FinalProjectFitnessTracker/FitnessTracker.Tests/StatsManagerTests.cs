namespace FitnessTracker.Tests;

using FitnessTracker;

public class StatsManagerTests
{
    StatsManager statsManager;
    WorkoutRoutine routine;

    public StatsManagerTests()
    {
        statsManager = new StatsManager();
        routine = new WorkoutRoutine("routine-1", "Morning Routine", new List<Exercise>());
    }

    [Fact]
    public void Test_GetStats_ReturnsOneEntryPerRoutine()
    {
        var sessions = new List<WorkoutSession>
        {
            MakeSession("session-1", "Bench Press: sets=3,reps=10"),
            MakeSession("session-2", "Bench Press: sets=3,reps=10"),
        };
        var stats = statsManager.GetStats(sessions);
        Assert.Single(stats);
        Assert.Equal("Morning Routine", stats[0].RoutineName);
    }

    [Fact]
    public void Test_GetStats_AggregatesTotalSetsAndReps()
    {
        var sessions = new List<WorkoutSession>
        {
            MakeSession("session-1", "Bench Press: sets=3,reps=10"),
            MakeSession("session-2", "Bench Press: sets=4,reps=12"),
        };
        var stats = statsManager.GetStats(sessions);
        Assert.Equal(7, stats[0].TotalSets);
        Assert.Equal(22, stats[0].TotalReps);
    }

    [Fact]
    public void Test_GetStats_AggregatesTotalMinutesAndDistance()
    {
        var sessions = new List<WorkoutSession>
        {
            MakeSession("session-1", "Running: miles=2,min=20"),
            MakeSession("session-2", "Running: miles=3,min=30"),
        };
        var stats = statsManager.GetStats(sessions);
        Assert.Equal(5, stats[0].TotalDistance);
        Assert.Equal(50, stats[0].TotalMinutes);
    }

    [Fact]
    public void Test_GetStats_CountsSessionsCorrectly()
    {
        var sessions = new List<WorkoutSession>
        {
            MakeSession("session-1", "Bench Press: sets=3,reps=10"),
            MakeSession("session-2", "Bench Press: sets=3,reps=10"),
            MakeSession("session-3", "Bench Press: sets=3,reps=10"),
        };
        var stats = statsManager.GetStats(sessions);
        Assert.Equal(3, stats[0].SessionCount);
    }

    [Fact]
    public void Test_GetStats_IgnoresSessionsWithNoNotes()
    {
        var session = MakeSession("session-1", "");
        var stats = statsManager.GetStats(new List<WorkoutSession> { session });
        Assert.Equal(0, stats[0].TotalSets);
    }

    // comparison feat
    [Fact]
    public void Test_GetTwoMostRecentByType_ReturnsTwoMostRecent()
    {
        var routine = MakeRoutine("Squat", ExerciseType.Strength);
        var sessions = new List<WorkoutSession>
        {
            MakeDatedSession("session-1", routine, new DateTime(2026, 4, 17)),
            MakeDatedSession("session-2", routine, new DateTime(2026, 4, 18)),
            MakeDatedSession("session-3", routine, new DateTime(2026, 4, 19)),
        };

        var result = statsManager.GetTwoMostRecentByType(sessions, ExerciseType.Strength);
        Assert.Equal(2, result.Count);
        Assert.Equal(new DateTime(2026, 4, 19), result[0].SessionDate);
        Assert.Equal(new DateTime(2026, 4, 18), result[1].SessionDate);
    }

    // helpers
    private WorkoutSession MakeSession(string id, string notes)
    {
        var session = new WorkoutSession(id, "user-1", DateTime.Now, routine);
        session.AddNotes(notes);
        return session;
    }

    private WorkoutSession MakeDatedSession(string id, WorkoutRoutine routine, DateTime date)                                                                   {                                                     
        return new WorkoutSession(id, "user-1", date, routine);
    }

    private WorkoutRoutine MakeRoutine(string exerciseName, ExerciseType type)               
    {    
        var exercise = new Exercise($"ex-{exerciseName}", exerciseName, "desc", type);
        return new WorkoutRoutine($"r-{exerciseName}", $"{exerciseName} Routine", new List<Exercise> { exercise });                                                                                 
    }
}
