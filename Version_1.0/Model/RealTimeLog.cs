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
        private List<RealTimeLogEntry> realTimeLog;
        private readonly string filePath;

        private RealTimeLog()
        {
            // The file will be created in the ApplicationData/EasySave/logs/state.json folder
            realTimeLog = new List<RealTimeLogEntry>();

            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySave", "logs");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            filePath = Path.Combine(directory, "state.json");
        }
        public static RealTimeLog getInstance()
        {
            if (instance == null)
            {
                instance = new RealTimeLog();
            }
            return instance;
        }

        public void CreateLogFile()
        {
            string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
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
        }

        public void SaveEntry(RealTimeLogEntry newEntry)
        {
            List<RealTimeLogEntry> existingLogs = new List<RealTimeLogEntry>();

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    existingLogs = JsonSerializer.Deserialize<List<RealTimeLogEntry>>(json) ?? new List<RealTimeLogEntry>();
                }
            }

            else
            {
                File.WriteAllText(filePath, "[]");
                existingLogs = new List<RealTimeLogEntry>();
            }

            existingLogs.Add(newEntry);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            File.WriteAllText(filePath, JsonSerializer.Serialize(existingLogs, options));
        }

        public void AddLogEntry(string name, string source, string target, string state, long TotalFilesToCopy, long TotalFilesSize, long NbFilesLeftToDo, int Progression)
        {
            RealTimeLogEntry entry = new RealTimeLogEntry
            {
                Name = name,
                Source = source,
                Target = target,
                State = state,
                TotalFilesToCopy = TotalFilesToCopy,
                TotalFilesSize = TotalFilesSize,
                NbFilesLeftToDo = NbFilesLeftToDo,
                Progression = Progression
            };
            
            SaveEntry(entry);
        }

        public List<RealTimeLogEntry> LoadRealTimeLog()
        {
            var realTimeLogs = new List<RealTimeLogEntry>();

            if (!File.Exists(filePath))
            {
                return realTimeLogs;
            }

            string json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return realTimeLogs;
            }

            // Deserialize to RealTimeLogEntry directly
            List<RealTimeLogEntry> entries = JsonSerializer.Deserialize<List<RealTimeLogEntry>>(json);

            if (entries != null)
            {
                foreach (var entry in entries)
                {
                    realTimeLogs.Add(entry);
                }
            }

            return realTimeLogs;
        }

        public void DeleteRealTimeLog()
        {
            if (!File.Exists(filePath))
                return;

            // Écrase le fichier avec un tableau JSON vide
            File.WriteAllText(filePath, "[]");
        }

        public void DeleteRealTimeLogEntry(string name)
        {
            var allLogs = LoadRealTimeLog();
            var logToDelete = allLogs.FirstOrDefault(log => log.Name == name);
            if (logToDelete != null)
            {
                allLogs.Remove(logToDelete);
                var options = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(filePath, JsonSerializer.Serialize(allLogs, options));
            }
        }

        public long TotalFilesToCopy(string source, string state)
        {
            if (Directory.Exists(source) && (state == "active" || state == "inactive"))
            {
                var files = Directory.GetFiles(source, "*", SearchOption.AllDirectories);
                return files.Length;
            }
            return 0;
        }

        public long TotalFilesSize(string source, string state)
        {
            long totalSize = 0;
            if (Directory.Exists(source) && (state == "active" || state == "inactive"))
            {
                var files = Directory.GetFiles(source, "*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    totalSize += fi.Length;
                }
                return totalSize;
            }
            return 0;
        }
        public long NbFilesLeftToDo(string source, string target, string state)
        {
                var filesSource = Directory.GetFiles(source, "*", SearchOption.AllDirectories);
                var filesTarget = Directory.GetFiles(target, "*", SearchOption.AllDirectories);
                return filesSource.Length - filesTarget.Length;
        }

        public int Progression(string source, string target)
        {
            var filesSource = Directory.GetFiles(source, "*", SearchOption.AllDirectories);
            var filesTarget = Directory.GetFiles(target, "*", SearchOption.AllDirectories);
            return filesTarget.Length / filesSource.Length *100;
        }

        public void UpdateStateFile(Work currentwork)
        {
            string name = currentwork.GetName();
            string source = currentwork.GetSource();
            string target = currentwork.GetTarget();
            string state = currentwork.GetState();
            long totalFilesToCopy = TotalFilesToCopy(source, state);
            long totalFilesSize = TotalFilesSize(source, state);
            long nbFilesLeftToDo = NbFilesLeftToDo(source, target, state);
            int progression = Progression(source, target);
            AddLogEntry(name, source, target, state, totalFilesToCopy, totalFilesSize, nbFilesLeftToDo, progression);
        }

        public class RealTimeLogEntry
        {
            public string Name { get; set; }
            public string Source { get; set; }
            public string Target { get; set; }
            public string State { get; set; }
            public long TotalFilesToCopy { get; set; }
            public long TotalFilesSize { get; set; }
            public long NbFilesLeftToDo { get; set; }
            public int Progression { get; set; }
        }
    }
}
