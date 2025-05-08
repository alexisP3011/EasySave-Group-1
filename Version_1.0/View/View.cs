using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version_1._0.Model;

namespace Version_1._0.View
{
    public class View
    {
        private readonly ViewModel.ViewModel _vm;

        public View()
        {
            _vm = new ViewModel.ViewModel();
        }

        public View(ViewModel.ViewModel viewModel)
        {
            _vm = viewModel;
        }

        public void ShowMenu()
        {
            Console.WriteLine("Welcome to the application!");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Create a backup work");
            Console.WriteLine("2. Show all the backup works");
            Console.WriteLine("3. Delete a backup work");
            Console.WriteLine("4. Update a backup work");
            Console.WriteLine("5. Launch a backup work");
            Console.WriteLine("6. Change the the application language");
            Console.WriteLine("7. Exit");

            Console.Write("Enter your choice: ");
        }

        public void ShowMessageChoice()
        {
            Console.WriteLine("Please select an option:");
        }

        public void AskConfirmation()
        {
            Console.WriteLine("Are you sure you want to do this operation? (y/n)");
        }

        public void WarnInputError()
        {
            Console.WriteLine("Invalid input. Please try again.");
        }

        public void ShowList(ViewModel.ViewModel viewModel)
        {
            var works = viewModel.GetAllWorks();
            if (works.Count == 0)
            {
                Console.WriteLine("No backup works available.");
            }
            else
            {
                Console.WriteLine("Here is the list of your backup works: ");
                int index = 1;
                foreach (var work in works)
                {
                    Console.WriteLine($"--- Work {index} ---");
                    Console.WriteLine($"Name: {work.name}");
                    Console.WriteLine($"Source: {work.source}");
                    Console.WriteLine($"Target: {work.target}");
                    Console.WriteLine($"Type: {work.type}");
                    Console.WriteLine($"State: {work.state}");
                    Console.WriteLine();
                    index++;
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public void ShowWork(Model.Work work)
        {
            Console.WriteLine("Here is the work details: ");
            Console.WriteLine($"Name: {work.name}");
            Console.WriteLine($"Source: {work.source}");
            Console.WriteLine($"Target: {work.target}");
            Console.WriteLine($"Type: {work.type}");
            Console.WriteLine($"State: {work.state}");
        }

        public void ShowCurrentWork(ViewModel.ViewModel viewModel)
        {
            Console.WriteLine("Current work details:");
            Console.WriteLine($"Name: {viewModel.GetName()}");
            Console.WriteLine($"Source: {viewModel.GetSource()}");
            Console.WriteLine($"Target: {viewModel.GetTarget()}");
            Console.WriteLine($"Type: {viewModel.GetType()}");
            Console.WriteLine($"State: {viewModel.GetState()}");
        }

        public string AskWorkName()
        {
            Console.WriteLine("Enter the name of the work:");
            return Console.ReadLine();
        }

        public void ShowLeaveMessage()
        {
            Console.WriteLine("Thank you for using the application. Goodbye!");
        }

        public void ShowWork()
        {
            Console.WriteLine("Here is the work you selected: ");
        }

        public void ShowNoWorkAvailable()
        {
            Console.WriteLine("No work available with this name.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public void ShowWorkCreatedMessage()
        {
            Console.WriteLine("Work created successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public void ShowWorkDeletedMessage()
        {
            Console.WriteLine("Work deleted successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public void ShowWorkUpdatedMessage()
        {
            Console.WriteLine("Work updated successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}