using System;
using Version_1._0.View;


namespace Version_1._0
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            var view = new View.View(); 

            while (!exit)
            {
                Console.Clear();
                view.ShowMenu();
                string? input = Console.ReadLine(); 
                switch (input)
                {
                    case "1":
                        // Create a backup work
                        break;
                    case "2":
                        // Show all the backup work
                        break;
                    case "3":
                        // Delete a backup work
                        break;
                    case "4":
                        // Update a backup work
                        break;
                    case "5":
                        // Launch a backup work
                        break;
                    case "6":
                        // Change the application language
                        break;
                    case "7":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }

            view.ShowLeaveMessage();
        }
    }
}