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
            Console.WriteLine("4. Workout Library");
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
                    if (dataManager.CurrentUser == null)
                    {
                        Console.WriteLine("Please select or create a user first.");
                        break;
                    }                
                    ExercisesLibraryMenu();
                    break;
                case "4":
                    // WorkoutLibraryMenu();
                    throw new NotImplementedException();
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
        Console.WriteLine("Create a new user");
        var userId = dataManager.GenerateUserId();
        Console.WriteLine($"Generated User Id: {userId}");

        Console.WriteLine("Enter User Name: ");
        var userName = Console.ReadLine();

        // validate user name input
        if (string.IsNullOrEmpty(userName))
        {
            Console.WriteLine("User Name cannot be empty. Please try again.");
            return;
        }
        if (dataManager.Users.Any(u => u.UserName == userName))
        {
            Console.WriteLine("User Name already exists. Please try again.");
            return;
        }


        // create user and save to data manager
        var newUser = new User(userId, userName);
        dataManager.Users.Add(newUser);
        dataManager.SaveUsers();
        dataManager.SetCurrentUser(newUser);

        Console.WriteLine($"User {userName} created successfully!");

        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
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

        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
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
                ShowMainMenu();
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }

    // exercise library methods

    public void AddExercise()
    {
        Console.WriteLine("Add a new exercise");
        var exerciseId = dataManager.GenerateExerciseId();
        Console.WriteLine($"Generated Exercise Id: {exerciseId}");


        Console.WriteLine("Enter Exercise Name: ");
        var exerciseName = Console.ReadLine()?.Trim();

        // validate exercise name input
        if (string.IsNullOrEmpty(exerciseName))
        {
            Console.WriteLine("Exercise Name cannot be empty. Please try again.");
            return;
        }
        if (dataManager.ExerciseLibrary.Any(e => e.ExerciseName == exerciseName))
        {
            Console.WriteLine("Exercise Name already exists. Please try again.");
            return;
        }

        Console.WriteLine("Enter Exercise Description: ");
        var exerciseDescription = Console.ReadLine();

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
        var newExercise = new Exercise(exerciseId, exerciseName, exerciseDescription, selectedExType);
        dataManager.ExerciseLibrary.Add(newExercise);
        dataManager.SaveExercises();

        Console.WriteLine($"Exercise {exerciseName} added successfully!");
        Console.WriteLine("Press any key to return to the Exercises Library menu...");
        Console.ReadKey();
    }

    public void ViewExercises()
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

        Console.WriteLine("Press any key to return to the Exercises Library menu...");
        Console.ReadKey();
    }
}