namespace FitnessTracker;

using Spectre.Console;


public class ConsoleUI
{
   
    DataManager dataManager;

    public ConsoleUI()
    {
        dataManager = new DataManager();
    }

// main menu    
      public void ShowMainMenu()
    {
        while (true)
        {
            var userDisplay = dataManager.CurrentUser != null
                ? $"[green]{dataManager.CurrentUser.UserName}[/]"
                : "[yellow]No user selected[/]";

            AnsiConsole.Clear();

            AnsiConsole.WriteLine();
            var rule = new Rule("[bold blue]Fitness Tracker[/]");
            AnsiConsole.Write(rule);
            AnsiConsole.MarkupLine($"Welcome! {userDisplay}");
            AnsiConsole.WriteLine();

            var menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Main Menu[/]")
                    .AddChoices(
                        "Create User",
                        "Select User",
                        "Exercises Library",
                        "Workout Routines Library",
                        "Start Workout",
                        "View Workout History",
                        "View Progress and Stats",
                        "Exit"
                    ));

            switch (menuChoice)
            {
                case "Exit":
                    AnsiConsole.MarkupLine("[grey]Exiting the app.[/]");
                    return;
                case "Create User":
                    CreateUser();
                    break;
                case "Select User":
                    SelectUser();
                    break;
                case "Exercises Library":
                    ExercisesLibraryMenu();
                    break;
                case "Workout Routines Library":
                    WorkoutRoutinesLibraryMenu();
                    break;
                case "Start Workout":
                    if (dataManager.CurrentUser == null)
                    {
                        AnsiConsole.MarkupLine("[red]Please select or create a user first.[/]");
                        Console.ReadKey(true);
                        break;
                    }
                    StartWorkoutRoutineMenu();
                    break;
                case "View Workout History":
                    if (dataManager.CurrentUser == null)
                    {
                        AnsiConsole.MarkupLine("[red]Please select or create a user first.[/]");
                        Console.ReadKey(true);
                        break;
                    }
                    ViewWorkoutHistory();
                    break;
                case "View Progress and Stats":
                    AnsiConsole.MarkupLine("[yellow]Progress and Stats feature is under development.[/]");
                    AnsiConsole.MarkupLine("[grey]Press any key to return to the main menu.[/]");
                    Console.ReadKey(true);
                    break;
            }
        }
    }

// user methods 
    private void CreateUser()
    {
        var userName = AnsiConsole.Ask<string>("Enter [green] User Name:[/]");

        if (string.IsNullOrEmpty(userName))
        {
            AnsiConsole.MarkupLine("[red]User Name cannot be empty. Please try again.[/]");
            return;
        }

        if (!dataManager.AddUser(dataManager.GenerateUserId(), userName))
        {
            AnsiConsole.MarkupLine("[red]User name already exists. Please try again.[/]");
            return;
        }
      
        dataManager.SetCurrentUser(dataManager.Users.Last());
        AnsiConsole.MarkupLine($"User {userName} created and selected.");
    }

    private void SelectUser()
    {
        if (dataManager.Users.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No users found. Please create a user first.[/]");
            return;
        }

        var selectedUser = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a user:")
                .AddChoices(dataManager.Users.Select(u => u.UserName)));

        var user = dataManager.Users.First(u => u.UserName == selectedUser);
        dataManager.SetCurrentUser(user);
        AnsiConsole.MarkupLine($"Selected User: {user.UserName}");
    }


    // exercise library menu 
    private void ExercisesLibraryMenu()
    {
        var exerciseLibraryMenuChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold]Exercises Library[/]")
                .AddChoices(
                    "Add Exercise",
                     "Remove Exercise",
                     "View Exercises",
                     "Back"
                ));

        switch (exerciseLibraryMenuChoice)
        {
            case "Add Exercise":
                AddExercise();
                break;
            case "Remove Exercise":
                // TODO: Remove Exercise 
                // RemoveExercise();
                AnsiConsole.MarkupLine("[yellow]Remove Exercise feature is under development.[/]");
                break;
            case "View Exercises":
                ViewExercises();
                break;
            case "Back":
                return;
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
     

        Console.WriteLine("Enter Exercise Description: "); // this could contain info like sets/reps/duration etc. TODO: add instructions for this input
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
            // TODO: remove workout routine
                Console.WriteLine("Remove Workout Routine feature is under development");
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


// start workout routine menu
    private void StartWorkoutRoutineMenu()
        {
            if (dataManager.WorkoutRoutines.Count == 0)
            {
                Console.WriteLine("No workout routines found. Create a workout routine first.");
                return;
            } 

            Console.WriteLine("Select a Workout Routine:");
            for (int i = 0; i < dataManager.WorkoutRoutines.Count; i++)
            {
                var routine = dataManager.WorkoutRoutines[i];
                Console.WriteLine($"{i + 1}. {routine.WorkoutRoutineName} ({routine.Exercises.Count} exercises)");
            }

            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= dataManager.WorkoutRoutines.Count)
            {
                var selectedRoutine = dataManager.WorkoutRoutines[choice - 1];
                dataManager.SetCurrentWorkoutRoutine(selectedRoutine);
                Console.WriteLine($"Selected Workout Routine: {selectedRoutine.WorkoutRoutineName}");

                Console.WriteLine("Exercises in this routine: ");
                foreach (var exercise in selectedRoutine.Exercises)
                {
                    Console.WriteLine($"- {exercise.ExerciseName} ({exercise.ExType})");
                }

                Console.Write("Mark this routine as completed? (y/n): ");
                var completionInput = Console.ReadLine()?.Trim().ToLower();
                if (completionInput == "y")
                {
                        Console.WriteLine("Enter date/time of the workout session (mm/dd/yyyy hh:mm), or press Enter for now: ");
                        var dateTimeInput = Console.ReadLine()?.Trim();
                        DateTime sessionDate;

                        if (!string.IsNullOrEmpty(dateTimeInput) && DateTime.TryParse(dateTimeInput, out DateTime parsedDate))
                        {
                            sessionDate = parsedDate;
                        }
                        else
                        {
                            sessionDate = DateTime.Now; 
                        }

                        var newSession = new WorkoutSession(dataManager.GenerateWorkoutSessionId(), sessionDate, selectedRoutine);
                        newSession.MarkSessionCompleted();
                        // TODO: hanlde notes input for workout session
                        

                        dataManager.AddWorkoutSession(newSession);
                        Console.WriteLine("Workout session recorded as completed!");
                    }
                    else
                    {
                        Console.WriteLine("Workout session not recorded.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid selection. Please try again.");
                }
        }

// Workout History
    private void ViewWorkoutHistory()
    {
        var sessions = dataManager.WorkoutSessions;

        if (sessions.Count == 0)
        {
            Console.WriteLine("No workout sessions found. Start a workout routine to record sessions.");
            return;
        }

        Console.WriteLine("Workout History:");
        foreach (var session in sessions)
        {
            Console.WriteLine($"Session ID: {session.SessionId}");
            Console.WriteLine($"Date: {session.SessionDate}");
            Console.WriteLine($"Routine: {session.Routine.WorkoutRoutineName}");
            Console.WriteLine("-----------------------------------");
        }
    }
}