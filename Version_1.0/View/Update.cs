using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version_1._0.View
{
    class Update : Add
    {
        public Update() { }

        public void AskItemToUpdate()
        {
            Console.WriteLine();
            Console.WriteLine("Please select the option you want to update: ");
            
            Console.WriteLine("1. Name of the backup work");
            Console.WriteLine("2. Source directory");
            Console.WriteLine("3. Target directory");
            Console.WriteLine("4. Type");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");
        }
    }
}
