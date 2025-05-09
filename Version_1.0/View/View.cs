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
            Console.WriteLine("6. Change the application language");
            Console.WriteLine("7. Exit");

            Console.Write("Enter your choice: ");
        }

        public void ShowMessageChoice()
        {
            Console.WriteLine();
            Console.WriteLine("Please select an option:");
        }

        public void AskConfirmation()
        {
            Console.WriteLine();
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
                    Console.WriteLine($"Name: {work.GetName()}");
                    Console.WriteLine($"Source: {work.GetSource()}");
                    Console.WriteLine($"Target: {work.GetTarget()}");
                    Console.WriteLine($"Type: {work.GetType()}");
                    Console.WriteLine($"State: {work.GetState()}");
                    Console.WriteLine();
                    index++;
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public void ShowWork(Model.Work work)
        {
            Console.WriteLine();
            Console.WriteLine("Here is the work details: ");
            Console.WriteLine($"Name: {work.GetName()}");
            Console.WriteLine($"Source: {work.GetSource()}");
            Console.WriteLine($"Target: {work.GetTarget()}");
            Console.WriteLine($"Type: {work.GetType()}");
            Console.WriteLine($"State: {work.GetState()}");
        }

        public void ShowCurrentWork(ViewModel.ViewModel viewModel)
        {
            Console.WriteLine();
            Console.WriteLine("Current work details:");
            Console.WriteLine($"Name: {viewModel.GetName()}");
            Console.WriteLine($"Source: {viewModel.GetSource()}");
            Console.WriteLine($"Target: {viewModel.GetTarget()}");
            Console.WriteLine($"Type: {viewModel.GetType()}");
            Console.WriteLine($"State: {viewModel.GetState()}");
        }

        public string AskWorkName()
        {
            Console.WriteLine();
            Console.WriteLine("Enter the name of the work:");
            return Console.ReadLine();
        }

        public void ShowLeaveMessage()
        {
            Console.WriteLine("Thank you for using the application. Goodbye!");
        }

        public void ShowWork()
        {
            Console.WriteLine();
            Console.WriteLine("Here is the work you selected: ");
        }

        public void ShowNoWorkAvailable()
        {
            Console.WriteLine("No work available with this name.");
            ShowNext();
        }

        public void ShowWorkCreatedMessage()
        {
            Console.WriteLine();
            Console.WriteLine("Work created successfully!");
            ShowNext();
        }

        public void ShowWorkDeletedMessage()
        {
            Console.WriteLine();
            Console.WriteLine("Work deleted successfully!");
            ShowNext();
        }

        public void ShowWorkUpdatedMessage()
        {
            Console.WriteLine();
            Console.WriteLine("Work updated successfully!");
            ShowNext();
        }
        public void ShowNext()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public void WarnMaxBackup()
        {
            Console.WriteLine();
            Console.WriteLine("You have reached the maximum number of backup works (5). Please delete one before creating a new one.");
            Console.ReadKey();
            Console.Clear();
        }
        public void WarnLaunch()
        {
            Console.WriteLine();
            Console.WriteLine("Launching the backup work...");
            ShowNext();
        }

        public void WarnInvalidOption()
        {
            Console.WriteLine();
            Console.WriteLine("Invalid option. Please try again.");
            ShowNext();
        }
    }
}