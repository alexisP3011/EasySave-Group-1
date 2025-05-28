using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Version_1._0.Model
{
    public class DailyLog
    {
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

        private string GetLogsDirectory(string baseDir = null)
        {
            string directoryPath = string.IsNullOrEmpty(baseDir)
                ? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                : baseDir;

            directoryPath = Path.Combine(directoryPath, "EasySave", "logs");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            return directoryPath;
        }

        public void createLogFile(string name = null, string directory = null)
        {
            string dir = GetLogsDirectory(directory);
            saveName = string.IsNullOrEmpty(name)
                ? Path.Combine(dir, $"{DateTime.Now:yyyy-MM-dd}.json")
                : Path.Combine(dir, name.EndsWith(".json", StringComparison.OrdinalIgnoreCase) ? name : name + ".json");

            if (!File.Exists(saveName))
            {
                File.WriteAllText(saveName, "[]");
                logEntries = new List<LogEntry>();
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

        public void createXmlLogFile(string name = null, string directory = null)
        {
            string dir = GetLogsDirectory(directory);
            saveName = string.IsNullOrEmpty(name)
                ? Path.Combine(dir, $"{DateTime.Now:yyyy-MM-dd}.xml")
                : Path.Combine(dir, name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase) ? name : name + ".xml");

            if (!File.Exists(saveName))
            {
                SaveLogsXml();
            }
            else
            {
                LoadLogsXml();
            }
        }

        public void SaveLogs()
        {
            if (string.IsNullOrEmpty(saveName))
                throw new InvalidOperationException("Le fichier de log JSON n'a pas été initialisé.");

            string json = JsonSerializer.Serialize(logEntries, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(saveName, json);
        }

        public void SaveLogsXml()
        {
            if (string.IsNullOrEmpty(saveName))
                throw new InvalidOperationException("Le fichier de log XML n'a pas été initialisé.");

            XmlSerializer serializer = new XmlSerializer(typeof(List<LogEntry>));
            using (FileStream fs = new FileStream(saveName, FileMode.Create))
            {
                serializer.Serialize(fs, logEntries);
            }
        }

        public void LoadLogsXml()
        {
            if (string.IsNullOrEmpty(saveName))
                return;

            if (File.Exists(saveName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<LogEntry>));
                using (FileStream fs = new FileStream(saveName, FileMode.Open))
                {
                    if (fs.Length == 0)
                    {
                        logEntries = new List<LogEntry>();
                    }
                    else
                    {
                        try
                        {
                            logEntries = (List<LogEntry>)serializer.Deserialize(fs);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erreur de désérialisation XML : " + ex.Message);
                            logEntries = new List<LogEntry>();
                        }
                    }
                }
            }
        }

        public int CountFilesInSource(string sourceDirectory, string searchPattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (string.IsNullOrEmpty(sourceDirectory) || !Directory.Exists(sourceDirectory))
                return 0;

            try
            {
                return Directory.GetFiles(sourceDirectory, searchPattern, searchOption).Length;
            }
            catch
            {
                return 0;
            }
        }

        public double GetFileSizeInKB(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return 0;

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                return Math.Round((double)fileInfo.Length / 1024, 2);
            }
            catch
            {
                return 0;
            }
        }

        public List<string> ListAllFiles(string directory, string searchPattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var filesList = new List<string>();

            if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
                return filesList;

            try
            {
                filesList.AddRange(Directory.GetFiles(directory, searchPattern, searchOption));
            }
            catch { }

            return filesList;
        }

        public void AddLogEntry(string jobName, string source, string destination, long size, double transferTime)
        {
            logEntries.Add(new LogEntry
            {
                Name = jobName,
                FileSource = source,
                FileTarget = destination,
                FileSize = size,
                FileTransferTime = transferTime,
                Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
            });
        }

        public void ClearLogs()
        {
            logEntries.Clear();
        }

        public List<LogEntry> GetLogEntries()
        {
            return logEntries;
        }

        public void TransferFilesWithLogs(string sourcePath, string destinationPath, string jobName = null, bool recursive = false, string format = null)
        {
            format = string.IsNullOrEmpty(format) ? Settings.Format : format.ToLower();

            if (!Directory.Exists(sourcePath))
            {
                Console.WriteLine($"Source directory {sourcePath} does not exist.");
                return;
            }

            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            // Nom du fichier log du jour avec extension
            string datedFileName = $"{DateTime.Now:yyyy-MM-dd}.{format}";

            if (format == "xml")
                createXmlLogFile(datedFileName);
            else
                createLogFile(datedFileName);

            jobName = datedFileName;

            SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            List<string> files = ListAllFiles(sourcePath, "*.*", searchOption);

            foreach (string file in files)
            {
                string relativePath = recursive
                    ? file.Substring(sourcePath.Length).TrimStart('\\', '/')
                    : Path.GetFileName(file);

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
                    double transferTimeSec = Math.Round(stopwatch.Elapsed.TotalMilliseconds / 1000, 3);

                    AddLogEntry(jobName, file, destFile, fileSize, transferTimeSec);

                    //Console.WriteLine($"Copied and logged: {file} -> {destFile}");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"Error copying file: {ex.Message}");
                }
            }

            if (format == "xml")
                SaveLogsXml();
            else
                SaveLogs();

            //Console.WriteLine($"Log saved to {saveName}");
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
}
