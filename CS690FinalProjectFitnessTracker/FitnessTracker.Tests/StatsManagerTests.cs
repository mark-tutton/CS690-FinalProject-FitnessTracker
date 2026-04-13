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

    private WorkoutSession MakeSession(string id, string notes)
    {
        var session = new WorkoutSession(id, "user-1", DateTime.Now, routine);
        session.AddNotes(notes);
        return session;
    }
}
