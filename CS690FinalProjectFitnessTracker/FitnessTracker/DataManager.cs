namespace FitnessTracker;

public class DataManager
{
    FileManager fileManager;

// session state
    public User? CurrentUser { get; private set; }
    public WorkoutRoutine ? CurrentWorkoutRoutine { get; private set; }

// data collections
    public List<User> Users { get; private set; }
    public List<Exercise> ExerciseLibrary { get; private set; }

    public List<WorkoutRoutine> WorkoutRoutines { get; private set; }

    public List<WorkoutSession> WorkoutSessions { get; private set; }

    // public List<ProgressTracker> ProgressTrackers { get; private set; }

    // public List<StatsTracker> StatsTrackers { get; private set; }

    public DataManager()
    {
        fileManager = new FileManager();

        Users = new List<User>();
        ExerciseLibrary = new List<Exercise>();
        WorkoutRoutines = new List<WorkoutRoutine>();
        WorkoutSessions = new List<WorkoutSession>();
        // ProgressTrackers = new List<ProgressTracker>();
        // StatsTrackers = new List<StatsTracker>();

        LoadUsers();
        LoadExercises();
        LoadWorkoutRoutines();
        LoadWorkoutSessions();
    }

// user management methods
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

    public void SaveUsers()
    {

        var data = string.Join(Environment.NewLine, Users.Select(user => $"{user.UserId}:{user.UserName}"));
        fileManager.OverwriteData("data/users.txt", data);
    }

    public void SetCurrentUser(User user)
    {
        CurrentUser = user;
    }

    public string GenerateUserId()
    {
        return $"user-{Guid.NewGuid()}";
    }

    public bool AddUser(string userId, string userName)
    {
        if (Users.Any(u => u.UserName == userName)) return false;
        var user = new User(userId, userName);
        Users.Add(user);
        SaveUsers();
        return true;
    }

    // remove user






// exercise library methods 
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

    public void SaveExercises()
    {
        var data = string.Join(Environment.NewLine, ExerciseLibrary.Select(ex => $"{ex.ExerciseId}:{ex.ExerciseName}:{ex.ExerciseDescription}:{ex.ExType}"));
        fileManager.OverwriteData("data/exercises.txt", data);
    }

    public string GenerateExerciseId()
    {
        return $"exercise-{Guid.NewGuid()}";
    }

    public bool AddExercise(Exercise exercise)
    {
        if (ExerciseLibrary.Any(e => e.ExerciseName == exercise.ExerciseName)) return false;
        ExerciseLibrary.Add(exercise);
        SaveExercises();
        return true;
    }

    public bool RemoveExercise(string exerciseId)
    {
        var exercise = ExerciseLibrary.FirstOrDefault(e => e.ExerciseId == exerciseId);
        if (exercise == null) return false;
        ExerciseLibrary.Remove(exercise);
        SaveExercises();
        return true;
    }

    
// workout routine methods
    private void LoadWorkoutRoutines()
    {
        try
        {
            var lines = fileManager.ReadAllLines("data/workoutRoutines.txt");
            foreach (var line in lines)
            {
                var parts = line.Split(":");
                if (parts.Length >= 3)
                {
                var routineId = parts[0];
                var routineName = parts[1];
                var exerciseIds = parts[2].Split(",");
               
                var exercises = exerciseIds
                    .Select(id => ExerciseLibrary.FirstOrDefault(e => e.ExerciseId == id.Trim()))
                    .ToList();
                
                                
                Console.WriteLine($"Loaded routine '{routineName}' with {exercises.Count} exercises");
                WorkoutRoutines.Add(new WorkoutRoutine(routineId, routineName, exercises));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading workout routines: {ex.Message}");
        }
    }

    public void SaveWorkoutRoutines()
    {
        var data = string.Join(Environment.NewLine, WorkoutRoutines.Select(wr => $"{wr.WorkoutRoutineId}:{wr.WorkoutRoutineName}:{string.Join(",", wr.Exercises.Select(e => e.ExerciseId))}"));
        fileManager.OverwriteData("data/workoutRoutines.txt", data);
    }

    public string GenerateWorkoutRoutineId()
    {
        return $"workoutRoutine-{Guid.NewGuid()}";
    }

    public bool AddWorkoutRoutine(WorkoutRoutine routine) {
        if (WorkoutRoutines.Any(r => r.WorkoutRoutineName == routine.WorkoutRoutineName)) return false;
        WorkoutRoutines.Add(routine);
        SaveWorkoutRoutines();
        return true;
    }

    public bool RemoveWorkoutRoutine(string workoutRoutineId)
    {
        var routine = WorkoutRoutines.FirstOrDefault(r => r.WorkoutRoutineId == workoutRoutineId);
        if (routine == null) return false;
        WorkoutRoutines.Remove(routine);
        SaveWorkoutRoutines();
        return true;
    }

    public void SetCurrentWorkoutRoutine(WorkoutRoutine routine) {
        CurrentWorkoutRoutine = routine;
    }

// workout session methods
    private void LoadWorkoutSessions()
    {
        try
        {
            var lines = fileManager.ReadAllLines("data/workoutSessions.txt");
            foreach (var line in lines)
            {
                var parts = line.Split(":");
                if (parts.Length >= 5)
                {
                    var sessionId = parts[0];
                    var routine = WorkoutRoutines.FirstOrDefault(r => r.WorkoutRoutineId == parts[1]);
                    var sessionDate = DateTime.Parse(parts[2]);
                    var sessionCompleted = bool.Parse(parts[3]);
                    var notes = parts[4];

                    if (routine != null)
                    {
                        var session = new WorkoutSession(sessionId, sessionDate, routine);
                        if (sessionCompleted) session.MarkSessionCompleted();
                        session.AddNotes(notes);
                        WorkoutSessions.Add(session);
                    }
                }
            }
        } catch (Exception ex)
        {
            Console.WriteLine($"Error loading workout sessions: {ex.Message}");
        }
    }

    private void SaveWorkoutSessions()
    {
        var data = string.Join(Environment.NewLine, WorkoutSessions.Select(ws => $"{ws.SessionId}:{ws.Routine.WorkoutRoutineId}:{ws.SessionDate}:{ws.IsCompleted}:{ws.Notes}"));
        fileManager.OverwriteData("data/workoutSessions.txt", data);
    }

    public string GenerateWorkoutSessionId()
    {
        return $"workoutSession-{Guid.NewGuid()}";
    }

    public void AddWorkoutSession(WorkoutSession session)
    {
        WorkoutSessions.Add(session);
        SaveWorkoutSessions();
    }

}