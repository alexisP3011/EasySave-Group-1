using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Version_3._0;

namespace Version_3._0
{
    public class WorkStorage
    {
        private List<WorkEntry> worksEntries;
        private static WorkStorage instance;
        private readonly string filePath;

        // Private constructor for Singleton
        private WorkStorage()
        {
            // The file will be created in the ApplicationData/EasySave/works.json folder
            worksEntries = new List<WorkEntry>();

            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySave");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            filePath = Path.Combine(directory, "works.json");
        }

        // Access to the unique instance
        public static WorkStorage getInstance()
        {
            if (instance == null)
            {
                instance = new WorkStorage();
            }

            return instance;

        }

        // Method to add a work entry to the JSON file without overwriting existing objects
        public void SaveWork(WorkEntry newEntry)
        {
            List<WorkEntry> existingWorks = new List<WorkEntry>();

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    existingWorks = JsonSerializer.Deserialize<List<WorkEntry>>(json) ?? new List<WorkEntry>();
                }
            }

            existingWorks.Add(newEntry);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            File.WriteAllText(filePath, JsonSerializer.Serialize(existingWorks, options));
        }

        // Method to load all works from the JSON file
        public List<Work> LoadAllWorks()
        {
            var works = new List<Work>();

            if (!File.Exists(filePath))
            {
                return works;
            }

            string json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return works;
            }

            // Deserialize to WorkEntry first, then convert to Work
            List<WorkEntry> entries = JsonSerializer.Deserialize<List<WorkEntry>>(json);

            if (entries != null)
            {
                foreach (var entry in entries)
                {


                    var work = new Work();
                    work.Name = entry.Name;
                    work.Source = entry.Source;
                    work.Target = entry.Target;
                    work.Type = entry.Type;
                    work.State = entry.State;
                    works.Add(work);

                }
            }

            return works;
        }


        public void AddWorkEntry(string name, string source, string target, string type, string state)
        {
            WorkEntry entry = new WorkEntry
            {
                Name = name,
                Source = source,
                Target = target,
                Type = type,
                State = state
            };
            // Save the work entry to the JSON file
            SaveWork(entry);
        }


        public int AllWorkCount()
        {
            var allWorks = LoadAllWorks();
            return allWorks?.Count ?? 0;
        }

        public void DeleteWorkEntry(string workToDelete)
        {
            if (!File.Exists(filePath))
                return;

            string json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json))
                return;

            List<WorkEntry> entries = JsonSerializer.Deserialize<List<WorkEntry>>(json) ?? new List<WorkEntry>();
            // Deletion by name
            entries.RemoveAll(e => e.Name == workToDelete);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            File.WriteAllText(filePath, JsonSerializer.Serialize(entries, options));
        }
    }

    public class WorkEntry
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
    }
}
