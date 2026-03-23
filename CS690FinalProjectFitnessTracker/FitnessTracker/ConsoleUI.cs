using System.Transactions;

namespace FitnessTracker;


public class ConsoleUI
{
   

   private readonly Dictionary<string, User> _users = new();
    public void ShowMainMenu()
    {
        while (true)
        {
            Console.WriteLine("Welcome to the Fitness Tracker!");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Add Exercise");
            Console.WriteLine("3. View Exercises");
            Console.WriteLine("4. Exit");

            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    CreateUser();
                    break;
                case "2":
                    ExercisesLibraryMenu();
                    break;
                case "3":
                    // WorkoutLibraryMenu();
                    throw new NotImplementedException();
                    break;
                case "4":
                    // StartWorkout();
                    throw new NotImplementedException();
                    break;
                case "5":
                    // HistoryMenu();
                    throw new NotImplementedException();
                    break;
                case "6":
                    // ProgressStatsMenu();
                    throw new NotImplementedException();
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private void CreateUser()
    {
        Console.WriteLine("Create a new user");
        Console.Write("Enter User Id: "); // TODO: this should be auto-generated
        var userId = Console.ReadLine();

        // validate user id input

        Console.WriteLine("Enter User Name: ");
        var userName = Console.ReadLine();

        // validate user name input


        // create user and add to dict // TODO: (this should be persisted in users.txt)

        var newUser = new User(userId, userName);
        _users.TryAdd(userId, newUser);

        Console.WriteLine($"User {userName} created successfully!");

        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
    }

    private void ExercisesLibraryMenu()
    {
        Console.WriteLine("Exercises Library");
        Console.WriteLine("1. Add Exercise");
        Console.WriteLine("2. Remove Exercise");
        Console.WriteLine("2. View Exercises");
        Console.WriteLine("4. Back to Main Menu");

        var input = Console.ReadLine();
        switch (input)
        {
            case "1":
                // TODO: Add Exercise
                // AddExercise();
                throw new NotImplementedException();
                break;
            case "2":   
                // TODO: Remove Exercise
                // RemoveExercise();
                throw new NotImplementedException();
                break;
            case "3":
                // TODO: View Exercises
                // ViewExercises();
                throw new NotImplementedException();
                break;
            case "4":
                ShowMainMenu();
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }
}