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
            Console.WriteLine("Please select the option you want to update: ");
            
            Console.WriteLine("1. Source directory");
            Console.WriteLine("2. Source target");
            Console.WriteLine("3. Type");
            Console.WriteLine("4. Exit");
        }
    }
}
