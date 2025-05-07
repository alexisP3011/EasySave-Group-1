using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version_1._0.View
{
    class View
    {

        public View() { }

        public void ShowMenu()
        {
            Console.WriteLine("Welcome to the application!");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Create a backup work");
            Console.WriteLine("2. Show all the backup work");
            Console.WriteLine("3. Delete a backup work");
            Console.WriteLine("4. Update a backup work");
            Console.WriteLine("5. Launch a backup work");
            Console.WriteLine("6. Change the the application language");
            Console.WriteLine("7. Exit");
            

            Console.Write("Enter your choice: ");
        }

    }
}
