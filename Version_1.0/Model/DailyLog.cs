using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Linq;

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

            if (Settings.Format.ToLower() == "xml")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<LogEntry>));
                using (FileStream fs = new FileStream(saveName.Replace(".json", ".xml"), FileMode.Create))
                {
                    serializer.Serialize(fs, logEntries);
                }
            }
            else
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string jsonString = JsonSerializer.Serialize(logEntries, options);
                File.WriteAllText(saveName, jsonString);
            }
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

        // New methods for exporting logs

        /// <summary>
        /// Exports the current logs to a JSON file at the specified path
        /// </summary>
        /// <param name="filePath">Full path for the output JSON file</param>
        public void ExportToJson(string filePath)
        {
            try
            {
                // Create directory if it doesn't exist
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Serialize with indentation for readability
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(logEntries, options);
                File.WriteAllText(filePath, json);

                Console.WriteLine($"Logs successfully exported to JSON: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting to JSON: {ex.Message}");
            }
        }

        /// <summary>
        /// Exports the current logs to an XML file at the specified path
        /// </summary>
        /// <param name="filePath">Full path for the output XML file</param>
        public void ExportToXml(string filePath)
        {
            try
            {
                // Create directory if it doesn't exist
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(List<LogEntry>));
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    serializer.Serialize(fs, logEntries);
                }

                Console.WriteLine($"Logs successfully exported to XML: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting to XML: {ex.Message}");
            }
        }

        /// <summary>
        /// Reads logs from a JSON file and returns them as a list of LogEntry objects
        /// </summary>
        /// <param name="filePath">Path to the JSON log file</param>
        /// <returns>List of LogEntry objects or null if reading fails</returns>
        public static List<LogEntry> ReadLogsFromJson(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Log file not found: {filePath}");
                    return null;
                }

                string jsonContent = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<LogEntry>>(jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading logs from JSON: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Gets summary statistics from the current log entries
        /// </summary>
        /// <returns>A dictionary containing statistics about the log entries</returns>
        public Dictionary<string, object> GetLogStatistics()
        {
            Dictionary<string, object> stats = new Dictionary<string, object>();

            if (logEntries == null || logEntries.Count == 0)
            {
                stats["totalEntries"] = 0;
                return stats;
            }

            stats["totalEntries"] = logEntries.Count;
            stats["totalFileSize"] = logEntries.Sum(e => e.FileSize);
            stats["averageFileSize"] = Math.Round(logEntries.Average(e => e.FileSize) / 1024.0, 2); // KB
            stats["totalTransferTime"] = Math.Round(logEntries.Sum(e => e.FileTransferTime), 2); // seconds
            stats["averageTransferTime"] = Math.Round(logEntries.Average(e => e.FileTransferTime), 2); // seconds

            // Get earliest and latest timestamps
            DateTime earliestTime = DateTime.MaxValue;
            DateTime latestTime = DateTime.MinValue;

            foreach (var entry in logEntries)
            {
                if (DateTime.TryParseExact(entry.Time, "dd/MM/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime entryTime))
                {
                    if (entryTime < earliestTime) earliestTime = entryTime;
                    if (entryTime > latestTime) latestTime = entryTime;
                }
            }

            stats["startTime"] = earliestTime == DateTime.MaxValue ? "Unknown" : earliestTime.ToString("dd/MM/yyyy HH:mm:ss");
            stats["endTime"] = latestTime == DateTime.MinValue ? "Unknown" : latestTime.ToString("dd/MM/yyyy HH:mm:ss");

            return stats;
        }
    }

    [Serializable] // Added to ensure proper XML serialization
    public class LogEntry
    {
        public LogEntry()
        {
            // Default constructor needed for serialization
        }

        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public long FileSize { get; set; }
        public double FileTransferTime { get; set; }
        public string Time { get; set; }
    }
}