using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version_1._0.View
{
    class Launch : View
    {
        public Launch() { }

        public void WarnAlreadyInProgress()
        {
            Console.WriteLine("A backup work is already underway");
        }
    }
}
