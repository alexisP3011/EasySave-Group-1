using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Runtime.CompilerServices;

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

        public void SaveWorkToJson()
        {
            string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = Path.Combine(directoryPath, "EasySave", "works.json");

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(filePath, JsonSerializer.Serialize(this, options));
        }

        public void LaunchWork()
        {
            if (this.GetState() == "inactive")
            {
                this.SetState("active");

                DailyLog logger = DailyLog.getInstance();
                logger.createLogFile();

                logger.TransferFilesWithLogs(
                                            this.GetSource(),  // Source path
                                            this.GetTarget(),  // Destination path
                                            this.GetName()     // Work name
                                            );

            }
            this.SetState("finished");

            //_works.Remove(workToLaunch);
        
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
