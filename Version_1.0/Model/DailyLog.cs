using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
namespace Version_1._0.Model
{
    public class DailyLog
    {
        // Singleton instance
        private static DailyLog instance;
        private string saveName;
        private List<LogEntry> logEntries;
        private DailyLog()
        {
            logEntries = new List<LogEntry>();
        }
        public static DailyLog getInstance()
        {
            if (instance == null)
            {
                instance = new DailyLog();
            }
            return instance;
        }
        public void createLogFile(string name = null, string directory = null)
        {
            string directoryPath = string.IsNullOrEmpty(directory)
                ? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads" 
                : directory;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if (string.IsNullOrEmpty(name))
            {

                saveName = Path.Combine(directoryPath, $"{DateTime.Now:yyyy-MM-dd}.json");
            }

            if (!File.Exists(saveName))
            {
                File.WriteAllText(saveName, "[]");
            }
            else
            {
                try
                {
                    string jsonContent = File.ReadAllText(saveName);
                    logEntries = JsonSerializer.Deserialize<List<LogEntry>>(jsonContent) ?? new List<LogEntry>();
                }
                catch
                {
                    logEntries = new List<LogEntry>();
                    File.WriteAllText(saveName, "[]");
                }
            }
        }

        public int CountFilesInSource(string sourceDirectory, string searchPattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (string.IsNullOrEmpty(sourceDirectory) || !Directory.Exists(sourceDirectory))
            {
                return 0;
            }

            try
            {
                return Directory.GetFiles(sourceDirectory, searchPattern, searchOption).Length;
            }
            catch (Exception ex)
            {
            
                return 0;
            }
        }

        public void AddLogEntry(string name, string source, string destination, long size, double transferTime)
        {
            LogEntry entry = new LogEntry
            {
                Name = name,
                FileSource = source,
                FileTarget = destination,
                FileSize = size,
                FileTransferTime = transferTime,
                Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
            };
            logEntries.Add(entry);
        }
        //public async Task SaveLogsAsync()
        //{
        //    if (string.IsNullOrEmpty(saveName))
        //    {
        //        createLogFile();
        //    }
        //    var options = new JsonSerializerOptions
        //    {
        //        WriteIndented = true
        //    };
        //    string jsonString = JsonSerializer.Serialize(logEntries, options);
        //    await File.WriteAllTextAsync(saveName, jsonString);
        //}
        public void SaveLogs()
        {
            if (string.IsNullOrEmpty(saveName))
            {
                createLogFile();
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(logEntries, options);
            File.WriteAllText(saveName, jsonString);
        }
        public void ClearLogs()
        {
            logEntries.Clear();
        }
        public List<LogEntry> GetLogEntries()
        {
            return logEntries;
        }
    }
    public class LogEntry
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public long FileSize { get; set; }
        public double FileTransferTime { get; set; }
        public string Time { get; set; }
    }
}