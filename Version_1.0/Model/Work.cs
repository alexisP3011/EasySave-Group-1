using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version_1._0.Model
{
    public class Work
    {
        private string name;
        private string source;
        private string target;
        private string type;
        private string state;
        
        public String GetName()
        {
            return this.name;
        }
        public void SetName(string input)
        {
            this.name = input;
        }
        public String GetSource()
        {
            return this.source;
        }
        public void SetSource(string input)
        {
            this.source = input;
        }
        public String GetTarget()
        {
            return this.target;
        }
        public void SetTarget(string input)
        {
            this.target = input;
        }
        public String GetType()
        {
            return this.type;
        }
        public void SetType(string input)
        {
            this.type = input;
        }
        public String GetState()
        {
            return this.state;
        }
        public void SetState(string input)
        {
            this.state = input;
        }

        public void LaunchWork()
        {
            if (this.GetState() == "inactive")
            {
                this.SetState("active");

                // Verify if the source directory exists
                if (Directory.Exists(this.GetSource()))
                {
                    // Create the target directory if it doesn't exist
                    if (!Directory.Exists(this.GetTarget()))
                    {
                        Directory.CreateDirectory(this.GetTarget());
                    }

                    // Copy all of the files to the new location
                    foreach (var file in Directory.GetFiles(this.GetSource()))
                    {
                        string fileName = Path.GetFileName(file);
                        string destFile = Path.Combine(this.GetTarget(), fileName);
                        File.Copy(file, destFile, overwrite: true);
                    }
                }
                this.SetState("finished");

                //_works.Remove(workToLaunch);
            }
        }


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
        


    }
}
