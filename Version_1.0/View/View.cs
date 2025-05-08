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
        private readonly ViewModel.ViewModel _vm = new ViewModel.ViewModel();

        public View() { }

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

        public void ShowList(ViewModel.ViewModel _vm)
        {
            Console.WriteLine("Here is the list of your backup works: ");
            Console.WriteLine($"Name: {_vm.GetName()}");
            Console.WriteLine($"Source: {_vm.GetSource()}");
            Console.WriteLine($"Target: {_vm.GetTarget()}");
            Console.WriteLine($"Type: {_vm.GetType()}");
            Console.WriteLine($"State: {_vm.GetState()}");

            Console.ReadKey();
        }

        public void ShowLeaveMessage()
        {
            Console.WriteLine("Thank you for using the application. Goodbye!");
        }

        public void ShowWork()
        {
            Console.WriteLine("Here is the work you selected: ");
        }
    }
}
