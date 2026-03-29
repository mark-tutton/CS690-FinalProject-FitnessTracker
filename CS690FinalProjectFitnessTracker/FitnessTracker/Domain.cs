// TODO: modularize by domain into Features dir

namespace FitnessTracker;

// User domain
public class User
{
    public string UserId { get; }
    public string UserName { get; }

    public User(string userId, string userName)
    {
        UserId = userId;
        UserName = userName;
    }
}

// Exercise domain
public enum ExerciseType { Cardio, Strength, Flexibility, Balance }

public class Exercise
{
    public string ExerciseId { get; }
    public string ExerciseName { get; }
    public string ExerciseDescription { get; }
    public ExerciseType ExType { get; }

    public Exercise(string exerciseId, string exerciseName, string exerciseDescription, ExerciseType exType)
    {
        ExerciseId = exerciseId;
        ExerciseName = exerciseName;
        ExerciseDescription = exerciseDescription;
        ExType = exType;
    }
}

// WorkoutRoutine domain
public class WorkoutRoutine
{
    public string WorkoutRoutineId { get; }
    public string WorkoutRoutineName { get; }
    public List<Exercise> Exercises { get; }

    public WorkoutRoutine(string workoutRoutineId, string workoutRoutineName, List<Exercise> exercises)
    {
        WorkoutRoutineId = workoutRoutineId;
        WorkoutRoutineName = workoutRoutineName;
        Exercises = exercises;
    }

    public void AddExercise(Exercise exercise)
    {
        Exercises.Add(exercise);
    }

    public void RemoveExercise(string exerciseId)
    {
        var exercise = Exercises.FirstOrDefault(e => e.ExerciseId == exerciseId);
        if (exercise != null)
        {
            Exercises.Remove(exercise);
        }
    }

   
}

// WorkoutSession domain
public class WorkoutSession
{
    public string SessionId { get; }
    public DateTime SessionDate { get; }
    public WorkoutRoutine Routine { get; }
    public string Notes { get; private set;}
    public bool IsCompleted { get; private set; }

    public WorkoutSession(string sessionId, DateTime sessionDate, WorkoutRoutine routine)
    {
        this.SessionId = sessionId;
        SessionDate = sessionDate;
        Routine = routine;
        Notes = "";
        IsCompleted = false;
    }

    public void MarkSessionCompleted()
    {
       IsCompleted = true;
    }

    public void AddNotes(string notes)
    {
        Notes = notes;
    }
}

// build out ProgressTracker

// build out ExerciseLog

