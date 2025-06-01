using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Globalization;
using System.Threading;

namespace Version_1._0.Model
{
    internal class Settings
    {
        private static Settings instance;
        private string settings_folder;
        private readonly string SETTINGS_FILE;

        private Settings()
        {
            settings_folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySave");
            SETTINGS_FILE = Path.Combine(settings_folder, "Settings.json");
        }

        public static Settings getInstance()
        {
            if (instance == null)
            {
                instance = new Settings();
            }
            return instance;
        }

        public string LoadSettingsLanguage()
        {
            Dictionary<string, string> settings;
            if (File.Exists(SETTINGS_FILE))
            {
                string jsonString = File.ReadAllText(SETTINGS_FILE);
                settings = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
            }
            else
            {
                settings = new Dictionary<string, string>();
                settings["language"] = "En";
                string jsonOutput = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                if (!Directory.Exists(settings_folder))
                {
                    Directory.CreateDirectory(settings_folder);
                }
                File.WriteAllText(SETTINGS_FILE, jsonOutput);
            }
            return settings["language"];
        }

        public void SaveSettings(string input)
        {
            Dictionary<string, string> settings;
            if (File.Exists(SETTINGS_FILE))
            {
                string jsonString = File.ReadAllText(SETTINGS_FILE);
                settings = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString) ?? new Dictionary<string, string>();
            }
            else
            {
                settings = new Dictionary<string, string>();
            }

            settings["language"] = input;

            string jsonOutput = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            if (!Directory.Exists(settings_folder))
            {
                Directory.CreateDirectory(settings_folder);
            }
            File.WriteAllText(SETTINGS_FILE, jsonOutput);
        }

        public void setCulture(string input)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(input);
            SaveSettings(input);
        }

        public void SaveLogFormat(string format)
        {
            Dictionary<string, string> settings;
            if (File.Exists(SETTINGS_FILE))
            {
                string jsonString = File.ReadAllText(SETTINGS_FILE);
                settings = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString) ?? new Dictionary<string, string>();
            }
            else
            {
                settings = new Dictionary<string, string>();
            }

            settings["logFormat"] = format;

            string jsonOutput = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            if (!Directory.Exists(settings_folder))
            {
                Directory.CreateDirectory(settings_folder);
            }
            File.WriteAllText(SETTINGS_FILE, jsonOutput);
        }

        public string LoadLogFormat()
        {
            if (File.Exists(SETTINGS_FILE))
            {
                string jsonString = File.ReadAllText(SETTINGS_FILE);
                var settings = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

                if (settings != null && settings.ContainsKey("logFormat"))
                {
                    return settings["logFormat"];
                }
            }

            // Valeur par défaut si non défini
            return "JSON";
        }

        // AJOUT : propriété statique pour récupérer facilement le format en minuscule
        public static string Format
        {
            get
            {
                return getInstance().LoadLogFormat().ToLower();
            }
        }
    }
}
