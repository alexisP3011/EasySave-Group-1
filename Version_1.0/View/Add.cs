using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version_1._0.View
{
    class Add : View
    {
        public Add() { }

        public void AskSaveName()
        {
            Console.WriteLine("Please enter the name of the backup work: ");
        }

        public void AskSource()
        {
            Console.WriteLine("Please enter your source: ");
        }
        public void AskTarget()
        {
            Console.WriteLine("Please enter your target: ");
        }

        public void AskType()
        {
            Console.WriteLine("Please enter the type of backup work you want to create: ");
        }


    }
}
