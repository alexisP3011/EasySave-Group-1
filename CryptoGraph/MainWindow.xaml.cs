using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Text.Json;
using System.Collections.Generic;
using Model;

namespace CryptoGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseFolder_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Choisissez un fichier dans le dossier à chiffrer",
                Filter = "Tous les fichiers (*.*)|*.*",
                CheckFileExists = true,
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFile = openFileDialog.FileName;
                string folder = Path.GetDirectoryName(selectedFile);
                FolderPathBox.Text = folder;
            }
        }


        private void EncryptAllButton_Click(object sender, RoutedEventArgs e)
        {
            string folder = FolderPathBox.Text;
            string extension = (ExtensionBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string key = KeyBox.Text;

            if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
            {
                MessageBox.Show("Veuillez sélectionner un dossier valide.", "Champ manquant", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(extension))
            {
                MessageBox.Show("Veuillez choisir une extension de fichier à chiffrer.", "Champ manquant", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                MessageBox.Show("Veuillez entrer une clé de chiffrement.", "Champ manquant", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var files = Directory.GetFiles(folder, "*" + extension);
            int count = 0;

            foreach (var file in files)
            {
                try
                {
                    var manager = new FileManager(file, key);
                    int result = manager.TransformFile();
                    if (result != -1)
                        count++;
                }
                catch
                {
                    // Ignorer ou journaliser les erreurs
                }
            }

            MessageBox.Show($"Chiffrement terminé. {count} fichier(s) traité(s).", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}