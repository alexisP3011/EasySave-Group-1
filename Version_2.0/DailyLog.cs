using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;





namespace Version_2._0
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
            ? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) : directory;
                        directoryPath = Path.Combine(directoryPath, "EasySave", "logs");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (string.IsNullOrEmpty(name))
            {
                saveName = Path.Combine(directoryPath, $"{DateTime.Now:yyyy-MM-dd}.json");
            }
            else
            {
                saveName = Path.Combine(directoryPath, name);
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

        public double GetFileSizeInKB(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return 0;
            }

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                return Math.Round((double)fileInfo.Length / 1024, 2);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<string> ListAllFiles(string directory, string searchPattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            List<string> filesList = new List<string>();

            if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
            {
                return filesList;
            }

            try
            {
                string[] files = Directory.GetFiles(directory, searchPattern, searchOption);
                filesList.AddRange(files);
                return filesList;
            }
            catch (Exception ex)
            {
                return filesList;
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


        public void TransferFilesWithLogs(string sourcePath, string destinationPath, string saveName = "Save1", bool recursive = false)
        {
            if (!Directory.Exists(sourcePath))
            {
                Console.WriteLine($"Source directory {sourcePath} does not exist.");
                return;
            }

            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }


            SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            List<string> files = ListAllFiles(sourcePath, "*.*", searchOption);

            foreach (string file in files)
            {
                string relativePath;
                if (recursive)
                {

                    relativePath = file.Substring(sourcePath.Length).TrimStart('\\', '/');
                }
                else
                {
                    relativePath = Path.GetFileName(file);
                }

                string destFile = Path.Combine(destinationPath, relativePath);


                string destDir = Path.GetDirectoryName(destFile);
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }


                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                try
                {

                    File.Copy(file, destFile, true);
                    Thread.Sleep(5000);
                    stopwatch.Stop();

                    long fileSize = new FileInfo(file).Length;


                    double transferTimeMs = stopwatch.Elapsed.TotalMilliseconds;
                    double transferTimeSec = Math.Round(transferTimeMs / 1000, 3);


                    AddLogEntry(
                        saveName,
                        file,
                        destFile,
                        fileSize,
                        transferTimeSec
                    );


                    SaveLogs();

                }
                catch (Exception ex)
                {
                }
            }

        }

        public void LogSaveError(string saveName, string processus)
        {
            LogEntry errorEntry = new LogEntry
            {
                Name = saveName,
                FileSource = "N/A",
                FileTarget = "N/A",
                FileSize = 0,
                FileTransferTime = 0,
                Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                ErrorMessage = "Error during launch because " + processus + " is launched"
            };

            logEntries.Add(errorEntry);
            SaveLogs();
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

        public string ErrorMessage { get; set; }  

    }
}