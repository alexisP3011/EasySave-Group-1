using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Version_2._0.Model;

namespace Version_2._0.View.Popup
{
    /// <summary>
    /// Logique d'interaction pour SettingsPopup.xaml
    /// </summary>
    public partial class SettingsPopup : Window
    {
        private readonly Settings _settings;

        public SettingsPopup()
        {
            InitializeComponent();
            _settings = Settings.Instance;
            LoadSettings();
        }

        private void LoadSettings()
        {
            // Charger les extensions autorisées
            EncryptionTextBox.Text = string.Join(", ", _settings.AllowedExtensions);

            // Charger la clé de cryptage
            KeyTextBox.Text = _settings.EncryptionKey;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer et valider les extensions
            List<string> extensions = EncryptionTextBox.Text
                .Split(',')
                .Select(ext => ext.Trim())
                .Where(ext => !string.IsNullOrWhiteSpace(ext))
                .Select(ext => ext.StartsWith(".") ? ext : "." + ext)
                .ToList();

            // Mettre à jour les paramètres
            _settings.AllowedExtensions = extensions;
            _settings.EncryptionKey = KeyTextBox.Text;

            // Sauvegarder les paramètres
            _settings.Save();

            MessageBox.Show("Paramètres sauvegardés avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}