using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Version_2._0.Model;
using System.IO;
using System.Text.Json;

namespace Version_2._0.View.Popup
{
    /// <summary>
    /// Logique d'interaction pour SettingsPopup.xaml
    /// </summary>
    public partial class SettingsPopup : Window
    {
        private readonly string SETTINGS_FILE;
        private readonly Settings _settings;

        public SettingsPopup()
        {
            InitializeComponent();
            string settings_folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            settings_folder = Path.Combine(settings_folder, "EasySave");


            SETTINGS_FILE = Path.Combine(settings_folder, "Settings_Easy_Save.json");

            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(SETTINGS_FILE))
                {
                    string jsonString = File.ReadAllText(SETTINGS_FILE);
                    var settings = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

                    EncryptionTextBox.Text = string.Join(", ", _settings.AllowedExtensions);
                    KeyTextBox.Text = _settings.EncryptionKey;

                    if (settings != null && settings.TryGetValue("Middleware", out string middlewareValue))
                    {
                        MiddlewareTextBox.Text = middlewareValue;
                        Software = middlewareValue;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string Software { get; set; }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Software = MiddlewareTextBox.Text;
            SaveSettings();
            this.Close();
        }

        

        private void SaveSettings()
        {
            try
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

                List<string> extensions = EncryptionTextBox.Text
                    .Split(',')
                    .Select(ext => ext.Trim())
                    .Where(ext => !string.IsNullOrWhiteSpace(ext))
                    .Select(ext => ext.StartsWith(".") ? ext : "." + ext)
                    .ToList();

                _settings.AllowedExtensions = extensions;
                _settings.EncryptionKey = KeyTextBox.Text;

                settings["Middleware"] = MiddlewareTextBox.Text;

                string jsonOutput = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SETTINGS_FILE, jsonOutput);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
