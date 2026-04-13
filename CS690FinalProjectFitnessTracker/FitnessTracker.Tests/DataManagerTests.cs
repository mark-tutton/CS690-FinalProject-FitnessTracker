namespace FitnessTracker.Tests;

using FitnessTracker;

public class DataManagerTests
{
    DataManager dataManager;

    public DataManagerTests()
    {
        Directory.CreateDirectory("data");
        File.WriteAllText("data/users.txt", "user-1:TestUser1" + Environment.NewLine + "user-2:TestUser2");
        File.WriteAllText("data/exercises.txt", "exercise-1:Bench Press:3 sets x 10 reps:Strength" + Environment.NewLine + "exercise-2:Running: 10 miles x 60 min:Cardio");
        File.WriteAllText("data/workoutRoutines.txt",
            "routine-1:Morning Routine:exercise-1,exercise-2");
        File.WriteAllText("data/workoutSessions.txt",
            "session-1:user-1:routine-1:2026-04-12 10-30-00:True:Test session note:Felt fatgigued but completed the workout");
        dataManager = new DataManager();
        dataManager.SetCurrentUser(dataManager.Users[0]);
    }

    // Users
    [Fact]
    public void Test_LoadUsers_LoadsTwoUsersFromFile()
    {
        Assert.Equal(2, dataManager.Users.Count);
    }

    [Fact]
    public void Test_AddUser_IncresesUserCount()
    {
        var initialCount = dataManager.Users.Count;
        var result = dataManager.AddUser(dataManager.GenerateUserId(), "NewTestUser");
        Assert.True(result);
        Assert.Equal(initialCount + 1, dataManager.Users.Count);
    }

    [Fact]
    public void Test_AddUser_ReturnsFalseForDuplicateName()
    {
        var result = dataManager.AddUser("user-3", "TestUser1");
        Assert.False(result);
        Assert.Equal(2, dataManager.Users.Count);
    }

    [Fact]
    public void Test_SetCurrentUser_SetsCurrentUser()
    {
        dataManager.SetCurrentUser(dataManager.Users[0]);
        Assert.Equal("TestUser1", dataManager.CurrentUser.UserName);
    }

    // Exercises
    [Fact]
    public void Test_LoadExercises_LoadsTwoExercisesFromFile()
    {
        Assert.Equal(2, dataManager.ExerciseLibrary.Count);
    }

    [Fact]
    public void Test_AddExercise_IncreasesExerciseCount()
    {
        var exercise = new Exercise("exercise-3", "Squats", "4 sets x 12 reps", ExerciseType.Strength);
        var result = dataManager.AddExercise(exercise);
        Assert.True(result);
        Assert.Equal(3, dataManager.ExerciseLibrary.Count);
    }

    [Fact]
    public void Test_AddExercise_ReturnsFalseForDuplicateName()
    {
        var exercise = new Exercise("exercise-3", "Bench Press", "3 sets x 10 reps", ExerciseType.Strength);
        var result = dataManager.AddExercise(exercise);
        Assert.False(result);
        Assert.Equal(2, dataManager.ExerciseLibrary.Count);
    }

    [Fact]
    public void Test_RemoveExercise_DecreasesCount()
    {
        var result = dataManager.RemoveExercise("exercise-1");
        Assert.True(result);
        Assert.Equal(1, dataManager.ExerciseLibrary.Count);
    }

    [Fact]
    public void Test_RemoveExercice_ReturnsFalseForNonExistentId()
    {
        var result = dataManager.RemoveExercise("exercise-04");
        Assert.False(result);
        Assert.Equal(2, dataManager.ExerciseLibrary.Count);
    }

    // Workout Routines
    [Fact]
    public void Test_LoadWorkoutRoutines_LoadsOneRoutineWithExercises()
    {
        Assert.Equal(1, dataManager.WorkoutRoutines.Count);
        Assert.Equal(2, dataManager.WorkoutRoutines[0].Exercises.Count);
    }

    [Fact]
    public void Test_AddWorkoutRoutine_IncreasesCount()
    {
        var routine = new WorkoutRoutine("routine-2", "Evening Routine", new List<Exercise> { dataManager.ExerciseLibrary[0] });
        var result = dataManager.AddWorkoutRoutine(routine);
        Assert.True(result);
        Assert.Equal(2, dataManager.WorkoutRoutines.Count);
    }       

    [Fact]                                                                            
    public void Test_RemoveWorkoutRoutine_DecreasesCount()
    {
        var result = dataManager.RemoveWorkoutRoutine("routine-1");
        Assert.True(result);
        Assert.Equal(0, dataManager.WorkoutRoutines.Count);
    }

    [Fact]
    public void Test_SetCurrentWorkoutRoutine_SetsCurrentRoutine()
    {
        dataManager.SetCurrentWorkoutRoutine(dataManager.WorkoutRoutines[0]);
        Assert.Equal("Morning Routine", dataManager.CurrentWorkoutRoutine.WorkoutRoutineName);
    }

    // Workout Sessions
    [Fact]
    public void Test_LoadWorkoutSessions_LoadsOneSessionFromFile()
    {
        Assert.Equal(1, dataManager.WorkoutSessions.Count);
        Assert.True(dataManager.WorkoutSessions[0].IsCompleted);   
    }

    public void Test_LoadWorkoutSessions_LoadsUserNotes()
    {
        Assert.Equal("Felt fatgigued but completed the workout", dataManager.WorkoutSessions[0].UserNotes);
    }

    [Fact]
    public void Test_AddWorkoutSession_IncreasesCount()
    {
        var session = new WorkoutSession("session-2", "user-1", DateTime.Now, dataManager.WorkoutRoutines[0]);
        dataManager.AddWorkoutSession(session);      
        Assert.Equal(2, dataManager.WorkoutSessions.Count);
    }
}