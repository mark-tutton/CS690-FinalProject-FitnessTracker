// TODO: modularize by domain into Features dir

namespace FitnessTracker;

public enum.ExerciseType { Cardio, Strength, Flexibility, Balance }

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