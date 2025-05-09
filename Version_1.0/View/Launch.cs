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
            if (language == "English")
            {
                Console.WriteLine("A backup work is already underway");
            }
            else if (language == "French")
            {
                Console.WriteLine("Un travail de sauvegarde est déjà en cours");
            }
        }
    }
}
