using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Version_2._0.Model
{
    public class Settings
    {
        // Extensions de fichiers à crypter
        public List<string> AllowedExtensions { get; set; } = new List<string> { ".txt", ".csv", ".json", ".xml", ".log" };

        // Clé de cryptage
        public string EncryptionKey { get; set; } = "defaultKey";

        // Chemin vers le fichier de configuration
        private readonly string _configPath = Path.Combine(
            Directory.GetCurrentDirectory(), "config.json");

        // Singleton pour les paramètres
        private static Settings _instance;
        public static Settings Instance
        {
            get => _instance ??= new Settings().Load();
        }

        // Charger les paramètres depuis le fichier de configuration
        public Settings Load()
        {
            if (File.Exists(_configPath))
            {
                try
                {
                    string json = File.ReadAllText(_configPath);
                    var settings = JsonSerializer.Deserialize<Settings>(json);
                    if (settings != null)
                    {
                        AllowedExtensions = settings.AllowedExtensions;
                        EncryptionKey = settings.EncryptionKey;
                    }
                }
                catch
                {
                    // En cas d'erreur, utiliser les valeurs par défaut
                }
            }
            return this;
        }

        // Sauvegarder les paramètres dans le fichier de configuration
        public void Save()
        {
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_configPath, json);
        }
    }
}