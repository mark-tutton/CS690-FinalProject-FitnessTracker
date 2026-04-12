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
        while (true) {
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
                RemoveExercise();
                break;
            case "View Exercises":
                ViewExercises();
                break;
            case "Back":
                return;
        }
        }
    }


    // exercise library methods 
    private void AddExercise()
    {
        var exerciseName = AnsiConsole.Ask<string>("Enter [green]Exercise Name[/]:");
        if (string.IsNullOrEmpty(exerciseName))
        {
            AnsiConsole.MarkupLine("[red]Exercise Name cannot be empty.[/]");
            return;
        }

        var exTypeChoice = AnsiConsole.Prompt(
            new SelectionPrompt<ExerciseType>()
                .Title("Select [green]Exercise Type[/]:")
                .AddChoices(Enum.GetValues<ExerciseType>()));

        string exerciseDescription;

        switch (exTypeChoice)
        {
            case ExerciseType.Strength:
            var sets = AnsiConsole.Ask<int>("Enter number of [green]sets[/]:");
            var reps = AnsiConsole.Ask<int>("Enter number of [green]reps[/]:");
            exerciseDescription = $"{sets} sets x {reps} reps";
            break;

            case ExerciseType.Cardio:
                var distance = AnsiConsole.Ask<string>("Enter [green]distance[/] (e.g. 20 miles):");
                var duration = AnsiConsole.Ask<string>("Enter [green]duration[/] (e.g. 60 mins):");
                exerciseDescription = $"{distance} / {duration}";
                if (AnsiConsole.Confirm("Is this an interval workout?"))
                {
                    var intervalSets = AnsiConsole.Ask<int>("Enter number of interval [green]sets[/]:");
                    var intervalReps = AnsiConsole.Ask<int>("Enter [green]reps[/] per set:");
                    exerciseDescription += $" | {intervalSets} sets x {intervalReps} reps";
                }
                break;

            case ExerciseType.Flexibility:
            case ExerciseType.Balance:
                var flexDuration = AnsiConsole.Ask<string>("Enter [green]duration[/] (e.g. 15 min):");
                exerciseDescription = flexDuration;
                break;

            default:
                exerciseDescription = AnsiConsole.Ask<string>("Enter [green]Exercise Description[/]:");
                break;
        }

        var newExercise = new Exercise(dataManager.GenerateExerciseId(), exerciseName, exerciseDescription, exTypeChoice);
        if (!dataManager.AddExercise(newExercise))
        {
            AnsiConsole.MarkupLine("[red]Exercise name already exists. Please try again.[/]");
            return;
        }

        AnsiConsole.MarkupLine($"[green]Exercise '{exerciseName}' added successfully![/]");
    }

    private void RemoveExercise()
    {
        if (dataManager.ExerciseLibrary.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No exercises found. Please add exercises first.[/]");
            return;
        }

        var exerciseToRemove = AnsiConsole.Prompt(
            new SelectionPrompt<string>() 
            .Title("Select an exercise to remove:")
            .AddChoices(dataManager.ExerciseLibrary.Select(e => $"{e.ExerciseName} ({e.ExType})")));
        
        var exercise = dataManager.ExerciseLibrary.First(e => $"{e.ExerciseName} ({e.ExType})" == exerciseToRemove);

        if (dataManager.RemoveExercise(exercise.ExerciseId))
        {
            AnsiConsole.MarkupLine($"[green]Exercise '{exercise.ExerciseName}' removed successfully![/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Failed to remove exercise. Please try again.[/]");
        }
    }
    private void ViewExercises()
    {
        if (dataManager.ExerciseLibrary.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No exercises found. Please add exercises first.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold]Exercises Library[/]")
            .AddColumn("ID")
            .AddColumn("Name")
            .AddColumn("Type")
            .AddColumn("Description");

        foreach (var exercise in dataManager.ExerciseLibrary)
        {
            table.AddRow(
                exercise.ExerciseId.ToString(),
                exercise.ExerciseName,
                exercise.ExType.ToString(),
                exercise.ExerciseDescription
            );
        }

        AnsiConsole.Write(table);
    }


// workout routines library menu
    public void WorkoutRoutinesLibraryMenu()
    {
        while (true)
        {
            var workoutRoutineLibraryMenuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Workout Routines Library[/]")
                    .AddChoices(
                        "Create Workout Routine",
                        "Remove Workout Routine",
                        "View Workout Routines",
                        "Back"
                    )
            );

            switch (workoutRoutineLibraryMenuChoice)
            {
                case "Create Workout Routine":
                    CreateWorkoutRoutine();
                    break;
                case "Remove Workout Routine":
                    RemoveWorkoutRoutine();
                    break;
                case "View Workout Routines":
                    ViewWorkoutRoutines();
                    break;
                case "Back":
                    return;
            }
        }
    }

// workout routines library methods
    private void CreateWorkoutRoutine()
    {
        var routineName = AnsiConsole.Ask<string>("Enter [green]Routine Name[/]:");
        if (string.IsNullOrEmpty(routineName))
        {
            AnsiConsole.MarkupLine("[red]Routine Name cannot be empty.[/]");
            return;
        }

        if (dataManager.ExerciseLibrary.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No exercises available. Add exercises to the library first.[/]");
            return;
        }

        var selectedExercises = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("Select [green]exercises[/] to add:")
                .AddChoices(dataManager.ExerciseLibrary.Select(e => $"{e.ExerciseName} ({e.ExType})")));

        var exercises = dataManager.ExerciseLibrary
            .Where(e => selectedExercises.Contains($"{e.ExerciseName} ({e.ExType})"))
            .ToList();

        var newWorkoutRoutine = new WorkoutRoutine(dataManager.GenerateWorkoutRoutineId(), routineName, exercises);
        if (!dataManager.AddWorkoutRoutine(newWorkoutRoutine))
        {
            AnsiConsole.MarkupLine("[red]Workout Routine name already exists. Please try again.[/]");
            return;
        }

        AnsiConsole.MarkupLine($"[green]Workout Routine '{routineName}' created.[/]");
    }
 

    private void RemoveWorkoutRoutine()
    {
        if (dataManager.WorkoutRoutines.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No workout routines found. Please create a workout routine first.[/]");
            return;
        }

        var routineToRemove = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a workout routine to remove:")
                .AddChoices(dataManager.WorkoutRoutines.Select(r => r.WorkoutRoutineName)));

        var routine = dataManager.WorkoutRoutines.First(r => r.WorkoutRoutineName == routineToRemove);

        if (dataManager.RemoveWorkoutRoutine(routine.WorkoutRoutineId))
        {
            AnsiConsole.MarkupLine($"[green]Workout Routine '{routine.WorkoutRoutineName}' removed successfully![/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Failed to remove workout routine. Please try again.[/]");
        }
    }
    private void ViewWorkoutRoutines()
    {
        if (dataManager.WorkoutRoutines.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No workout routines found. Please create a workout routine first.[/]");
            return;
        }

        foreach (var routine in dataManager.WorkoutRoutines)
        {
            var routinesPanel = new Panel(
                new Rows(
                    new Markup($"[bold]ID:[/] {routine.WorkoutRoutineId}"),
                    new Markup($"[bold]Exercises:[/]"),
                    routine.Exercises?.Count > 0
                        ? new Rows(routine.Exercises
                            .Where(e => e != null)
                            .Select(e => new Markup($"  • {e.ExerciseName} ({e.ExType})")))
                        : new Rows(new Markup("[grey]No exercises in this routine.[/]"))
                ))
                .Header($"[blue]{routine.WorkoutRoutineName}[/]")
                .Border(BoxBorder.Rounded);

            AnsiConsole.Write(routinesPanel);
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