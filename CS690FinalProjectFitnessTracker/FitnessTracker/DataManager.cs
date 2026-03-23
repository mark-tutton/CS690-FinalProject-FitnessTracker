namespace FitnessTracker;

public class DataManager
{
    FileManager fileManager;

    public User? CurrentUser { get; private set; }

    public List<User> Users { get; private set; }
    public List<Exercise> ExerciseLibrary { get; private set; }

    // public List<WorkoutRoutine> WorkoutRoutines { get; private set; }

    // public List<WorkoutSession> WorkoutSessions { get; private set; }

    // public List<ProgressTracker> ProgressTrackers { get; private set; }

    // public List<StatsTracker> StatsTrackers { get; private set; }

    public DataManager()
    {
        fileManager = new FileManager();

        Users = new List<User>();
        ExerciseLibrary = new List<Exercise>();
        // WorkoutRoutines = new List<WorkoutRoutine>();
        // WorkoutSessions = new List<WorkoutSession>();
        // ProgressTrackers = new List<ProgressTracker>();
        // StatsTrackers = new List<StatsTracker>();

        LoadUsers();
        LoadExercises();
        // LoadWorkoutRoutines();
    }

    public void LoadUsers()
    {
        try
        {
            var lines = fileManager.ReadAllLines("data/users.txt");
            foreach (var line in lines)
            {
                var parts = line.Split(':');
                if (parts.Length >= 2)
                {
                    Users.Add(new User(parts[0], parts[1]));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading users: {ex.Message}");
        }
    }

    public void SetCurrentUser(User user)
    {
        CurrentUser = user;
    }

    private void LoadExercises()
    {
        try
        {
            var lines = fileManager.ReadAllLines("data/exercises.txt");
            foreach (var line in lines)
            {
                var parts = line.Split(':');
                if (parts.Length >= 4)
                {
                    ExerciseType exType;
                    if (Enum.TryParse(parts[3], out exType))
                    {
                        ExerciseLibrary.Add(new Exercise(parts[0], parts[1], parts[2], exType));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading exercises: {ex.Message}");
        }
    }

    // private void LoadWorkoutRoutines()
    // {
    // }

    public void SaveUsers()
    {

        var data = string.Join(Environment.NewLine, Users.Select(user => $"{user.UserId}:{user.UserName}"));
        fileManager.OverwriteData("data/users.txt", data);
    }

}