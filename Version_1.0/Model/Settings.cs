using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.ComponentModel;
using System.Globalization;

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
            File.WriteAllText(SETTINGS_FILE, jsonOutput);

        }
        public void setCulture(string input)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(input);
            SaveSettings(input);
        }
    }
}
