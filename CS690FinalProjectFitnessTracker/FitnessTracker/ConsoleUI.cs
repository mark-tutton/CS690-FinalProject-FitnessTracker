namespace FitnessTracker;


public class ConsoleUI
{
   
    DataManager dataManager;

    public ConsoleUI()
    {
        dataManager = new DataManager();
    }
    public void ShowMainMenu()
    {
        while (true)
        {
            var userDisplay = dataManager.CurrentUser != null
                ? $"Current User: {dataManager.CurrentUser.UserName}"
                : "No user selected";


            Console.WriteLine("Welcome to the Fitness Tracker, " + userDisplay + "!");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Select User");
            Console.WriteLine("3. Exercises Library");
            Console.WriteLine("4. Workout Routines Library");
            Console.WriteLine("5. Start Workout");
            Console.WriteLine("6. View Workout History");
            Console.WriteLine("7. View Progress and Stats");
            Console.WriteLine("0. Exit");

            var input = Console.ReadLine();
            switch (input)
            {
                case "0":
                    Console.WriteLine("Exiting the app.");
                    return;
                case "1":
                    CreateUser();
                    break;
                case "2":
                    SelectUser();
                    break;
                case "3":
                    // if (dataManager.CurrentUser == null)
                    // {
                    //     Console.WriteLine("Please select or create a user first.");
                    //     break;
                    // }                
                    ExercisesLibraryMenu();
                    break;
                case "4":
                    WorkoutRoutinesLibraryMenu();
                    break;
                case "5":
                    // StartWorkout();
                    throw new NotImplementedException();
                    break;
                case "6":
                    // HistoryMenu();
                    throw new NotImplementedException();
                    break;
                case "7":
                    // ProgressStatsMenu();
                    throw new NotImplementedException();
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

// user methods 
    private void CreateUser()
    {
        Console.WriteLine("Enter User Name: ");
        var userName = Console.ReadLine();

        if (string.IsNullOrEmpty(userName))
        {
            Console.WriteLine("User Name cannot be empty. Please try again.");
            return;
        }

        if (!dataManager.AddUser(dataManager.GenerateUserId(), userName))
        {
            Console.WriteLine("User name already exists. Please try again.");
            return;
        }
      
        dataManager.SetCurrentUser(dataManager.Users.Last());
        Console.WriteLine($"User {userName} created and selected.");
    }

    private void SelectUser()
    {
        if (dataManager.Users.Count == 0)
        {
            Console.WriteLine("No users found. Please create a user first.");
            return;
        }

        Console.WriteLine("Select a user:");
        for (int i = 0; i < dataManager.Users.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {dataManager.Users[i].UserName}");
        }

        if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= dataManager.Users.Count)
        {
            dataManager.SetCurrentUser(dataManager.Users[choice - 1]);
            Console.WriteLine($"Selected User {dataManager.CurrentUser.UserName}.");
        }
        else
        {
            Console.WriteLine("Invalid selection. Please try again.");
        }
    }


    // exercise library menu 
    private void ExercisesLibraryMenu()
    {
        Console.WriteLine("Exercises Library");
        Console.WriteLine("1. Add Exercise");
        Console.WriteLine("2. Remove Exercise");
        Console.WriteLine("3. View Exercises");
        Console.WriteLine("4. Back to Main Menu");

        var input = Console.ReadLine();
        switch (input)
        {
            case "1":
                // TODO: Add Exercise
                AddExercise();
                break;
            case "2":
                // TODO: Remove Exercise
                // RemoveExercise();
                throw new NotImplementedException();
                break;
            case "3":
                // TODO: View Exercises
                ViewExercises();
                break;
            case "4":
                return;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }

    // exercise library methods 

    private void AddExercise()
    {
        Console.WriteLine("Enter Exercise Name: ");
        var exerciseName = Console.ReadLine()?.Trim();

        // validate exercise name input
        if (string.IsNullOrEmpty(exerciseName))
        {
            Console.WriteLine("Exercise Name cannot be empty. Please try again.");
            return;
        }
     

        Console.WriteLine("Enter Exercise Description: ");
        var exerciseDescription = Console.ReadLine()?.Trim();

        // validate exercise description input
        if (string.IsNullOrEmpty(exerciseDescription))
        {
            Console.WriteLine("Exercise Description cannot be empty. Please try again.");
            return;
        }

        Console.Write("Enter Exercise Type (0.Cardio, 1.Strength, 2.Flexibility, 3.Balance): ");

        var exerciseTypeInput = Console.ReadLine()?.Trim();
        if (!int.TryParse(exerciseTypeInput, out int exTypeChoice) || !Enum.IsDefined(typeof(ExerciseType), exTypeChoice))
        {
            Console.WriteLine("Invalid Exercise Type selection. Please try again.");
            return;
        }
        var selectedExType = (ExerciseType)exTypeChoice;

        // create exercise and save to data manager
        var newExercise = new Exercise(dataManager.GenerateExerciseId(), exerciseName, exerciseDescription, selectedExType);
        if (!dataManager.AddExercise(newExercise))
        {
            Console.WriteLine("Exercise name already exists. Please try again.");
            return;
        }

        Console.WriteLine($"Exercise {exerciseName} added successfully!");
    }

    private void ViewExercises()
    {
        if (dataManager.ExerciseLibrary.Count == 0)
        {
            Console.WriteLine("No exercises found. Please add exercises first.");
            return;
        }

        Console.WriteLine("Exercises Library:");
        foreach (var exercise in dataManager.ExerciseLibrary)
        {
            Console.WriteLine($"ID: {exercise.ExerciseId}, Name: {exercise.ExerciseName}, Type: {exercise.ExType}");
            Console.WriteLine($"Description: {exercise.ExerciseDescription}");
            Console.WriteLine("-----------------------------------");
        }
    }

// workout routines library menu
    public void WorkoutRoutinesLibraryMenu()
    {
        Console.WriteLine("Workout Routines Library");
        Console.WriteLine("1. Create Workout Routine");
        Console.WriteLine("2. Remove Workout Routine");
        Console.WriteLine("3. View Workout Routines");
        Console.WriteLine("4. Back to Main Menu");

        var input = Console.ReadLine();
        switch (input)
        {
            case "1":
                CreateWorkoutRoutine();
                break;
            case "2":
            // remove workout routine
                throw new NotImplementedException();
                break;
            case "3":
                ViewWorkoutRoutines();
                break;
            case "4":
                return;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }


// workout routines library methods
    private void CreateWorkoutRoutine()
    {
        Console.Write("Enter Routine Name: ");
        var routineName = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(routineName))
        {
            Console.WriteLine("Routine Name cannot be empty. Please try again.");
            return;
        }

        if (dataManager.ExerciseLibrary.Count == 0)
        {
            Console.WriteLine("No exercises available. Add exercises to the library first.");
            return;
        }

        // display existing workouts
        Console.WriteLine("Available Exercises:");
        for (int i = 0; i < dataManager.ExerciseLibrary.Count; i++)
        {
            var exercise = dataManager.ExerciseLibrary[i];
            Console.WriteLine($"{i + 1}. {exercise.ExerciseName} ({exercise.ExType})");
        }

        Console.WriteLine("Select exercises to add (Enter numbers separated by commas): ");
        var exerciseSelection = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(exerciseSelection))
        {
            Console.WriteLine("No exercises selected. Please try again.");
            return;
        }

        // select exercises 
        var selectedExercises = new List<Exercise>();

        foreach (var selection in exerciseSelection.Split(","))
        {
            if (int.TryParse(selection.Trim(), out int index) && index >= 1 && index <= dataManager.ExerciseLibrary.Count)
            {
                selectedExercises.Add(dataManager.ExerciseLibrary[index - 1]);
                Console.WriteLine($"Added {dataManager.ExerciseLibrary[index - 1].ExerciseName}");
            }
            else
            {
                Console.WriteLine($"Invalid selection: {selection}");
            }
        }

        var newWorkoutRoutine = new WorkoutRoutine(dataManager.GenerateWorkoutRoutineId(), routineName, selectedExercises);
        if (!dataManager.AddWorkoutRoutine(newWorkoutRoutine))
        {
            Console.WriteLine("Workout Routine name already exists. Please try again.");
            return;
        }   

        Console.WriteLine($"Workout Routine {routineName} created.");
    }

    private void ViewWorkoutRoutines()
    {
        if (dataManager.WorkoutRoutines.Count == 0)
        {
            Console.WriteLine("No workout routines found. Please create a workout routine first.");
            return;
        }

        Console.WriteLine("Workout Routines:");
        foreach (var routine in dataManager.WorkoutRoutines)
        {
            Console.WriteLine($"ID: {routine.WorkoutRoutineId}, Name: {routine.WorkoutRoutineName}");
            Console.WriteLine("Exercises:");
            if (routine.Exercises != null && routine.Exercises.Count > 0)
            {
                foreach (var exercise in routine.Exercises)
                {
                    if (exercise != null) {
                        Console.WriteLine($"- {exercise.ExerciseName} ({exercise.ExType})");
                    }
                }
            } 
            else
            {
                Console.WriteLine("- No exercises in this routine.");
            }
            Console.WriteLine("-----------------------------------");
        }
    }

}