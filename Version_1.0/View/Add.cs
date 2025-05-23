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
            Console.WriteLine();
            Console.WriteLine(_rm.GetString("AskSaveName"));
        }

        public void AskSource()
        {
            Console.WriteLine();
            Console.WriteLine(_rm.GetString("AskSource"));
        }
        public void AskTarget()
        {
            Console.WriteLine();
            Console.WriteLine(_rm.GetString("AskTarget"));
        }

        public void AskType()
        {
            Console.WriteLine();
                Console.WriteLine(_rm.GetString("AskType"));
        }
    }
}
