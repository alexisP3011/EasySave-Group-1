using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version_1._0.Model
{
    public class Work
    {
        public Work() {
            name = "";
            source = "";
            target = "";
            type = "";
            state = "inactive";     

        }

        ~Work() // Destructor
        {
            
        }

        public string name { get; set; }
        public string source { get; set; }
        public string target { get; set; }
        public string type { get; set; }
        public string state { get; set; }

        

        

    }
}
