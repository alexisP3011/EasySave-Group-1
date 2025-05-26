using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Text.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace Version_2._0.View.Popup
{
    /// <summary>
    /// Logique d'interaction pour SettingsPopup.xaml
    /// </summary>
    public partial class SettingsPopup : Window
    {
        private readonly string SETTINGS_FILE;
        private ResourceManager _rm = new ResourceManager("Version_2._0.Ressources.string", typeof(SettingsPopup).Assembly);

        public SettingsPopup()
        {
            InitializeComponent();

            string settings_folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            settings_folder = Path.Combine(settings_folder, "EasySave");

            SETTINGS_FILE = Path.Combine(settings_folder, "Settings_Easy_Save.json");

            LoadSettings();

            SettingsTitle.Text = _rm.GetString("SettingsPopUp");
            LanguageTextBlock.Text = _rm.GetString("LanguageLabel");
            MiddlewareTextBlock.Text = _rm.GetString("MiddlewareLabel");
            ExtensionEncryptionTextBlock.Text = _rm.GetString("ExtensionLabel");
            KeyEncryptionTextBlock.Text = _rm.GetString("KeyLabel");
            ExtensionLogTextBlock.Text = _rm.GetString("LogExtensionLabel");
            ConfirmButton.ApplyTemplate();
            TextBlock confirmTextBlock = ConfirmButton.Template.FindName("ConfirmButtonText", ConfirmButton) as TextBlock;
            confirmTextBlock.Text = _rm.GetString("ConfirmButton");
            CancelButton.ApplyTemplate();
            TextBlock cancelTextBlock = CancelButton.Template.FindName("CancelButtonText", CancelButton) as TextBlock;
            cancelTextBlock.Text = _rm.GetString("CancelButton");
        }

        public string Software { get; set; }
        public string TargetExtension { get; set; }
        public string EncryptionKey { get; set; }
        public string Language { get; set; }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Software = MiddlewareTextBox.Text;
            
            if (ExtensionComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                TargetExtension = selectedItem.Content.ToString();
            }

            EncryptionKey = EncryptionKeyTextBox.Text;
            
            if (LanguageComboBox.SelectedItem is ComboBoxItem langItem)
            {
                Language = langItem.Content.ToString();
            }

            SaveSettings();
            this.Close();
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(SETTINGS_FILE))
                {
                    string jsonString = File.ReadAllText(SETTINGS_FILE);
                    var settings = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

                    if (settings != null)
                    {
                        if (settings.TryGetValue("Middleware", out string middlewareValue))
                        {
                            MiddlewareTextBox.Text = middlewareValue;
                            Software = middlewareValue;
                        }

                        if (settings.TryGetValue("TargetExtension", out string targetExtensionValue))
                        {
                            TargetExtension = targetExtensionValue;

                            foreach (ComboBoxItem item in ExtensionComboBox.Items)
                            {
                                if (item.Content.ToString() == targetExtensionValue)
                                {
                                    ExtensionComboBox.SelectedItem = item;
                                    break;
                                }
                            }
                        }

                        if (settings.TryGetValue("EncryptionKey", out string encryptionKeyValue))
                        {
                            EncryptionKeyTextBox.Text = encryptionKeyValue;
                            EncryptionKey = encryptionKeyValue;
                        }

                        if (settings.TryGetValue("Language", out string languageValue))
                        {
                            Language = languageValue;
                            foreach (ComboBoxItem item in LanguageComboBox.Items)
                            {
                                if (item.Content.ToString() == languageValue)
                                {
                                    LanguageComboBox.SelectedItem = item;
                                    setCulture(languageValue);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

  
                settings["Middleware"] = MiddlewareTextBox.Text;
                settings["TargetExtension"] = TargetExtension;
                settings["EncryptionKey"] = EncryptionKeyTextBox.Text;
                settings["Language"] = Language;


                string jsonOutput = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SETTINGS_FILE, jsonOutput);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void setCulture(string language)
        {
            if (language == "English")
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("En");
            }
            else if (language == "Français")
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("Fr");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}