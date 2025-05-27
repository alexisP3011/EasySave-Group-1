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
            Console.WriteLine(_rm.GetString("AskUpdate"));
            Console.Write(_rm.GetString("UpdateChoices"));
            Console.Write(_rm.GetString("EnterChoice"));
        }
    }
}
