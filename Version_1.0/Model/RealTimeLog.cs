using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Version_1._0.Model
{
    internal class RealTimeLog
    {
        // Singleton instance
        private static RealTimeLog instance;
        private string saveName;

        public static RealTimeLog getInstance()
        {
            if (instance == null)
            {
                instance = new RealTimeLog();
            }
            return instance;
        }

        public void createLogFile(string directory = null)
        {
            string directoryPath = string.IsNullOrEmpty(directory) ? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) : directory;
            directoryPath = Path.Combine(directoryPath, "EasySave", "logs");
            string saveName = Path.Combine(directoryPath, "state.json");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (!File.Exists(saveName))
            {
                File.WriteAllText(saveName, "[]");
            }

            else
            {
                string jsonContent = File.ReadAllText(saveName);
                List<LogEntry> logEntries = JsonSerializer.Deserialize<List<LogEntry>>(jsonContent) ?? new List<LogEntry>();
            }

        }
    }
}
