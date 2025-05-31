using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

        private async void EncryptAllButton_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = FolderPathBox.Text.Trim();

            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("Le dossier spécifié n'existe pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ExtensionBox.SelectedItem is not ComboBoxItem selectedItem)
            {
                MessageBox.Show("Veuillez sélectionner une extension.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string extension = selectedItem.Content.ToString();
            string key = KeyBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(key))
            {
                MessageBox.Show("Veuillez entrer une clé de chiffrement.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string[] files = Directory.GetFiles(folderPath, $"*{extension}", SearchOption.AllDirectories);

            if (files.Length == 0)
            {
                MessageBox.Show($"Aucun fichier avec l'extension {extension} trouvé dans le dossier spécifié.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string exeRelativePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CryptoSoft.exe");
            string exeFullPath = Path.GetFullPath(exeRelativePath);

            if (!File.Exists(exeFullPath))
            {
                MessageBox.Show("Le fichier CryptoSoft.exe n'a pas été trouvé dans le répertoire de l'application.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int successCount = 0;
            int errorCount = 0;
            List<string> errorDetails = new List<string>();

            foreach (string file in files)
            {
                var psi = new ProcessStartInfo
                {
                    FileName = exeFullPath,
                    Arguments = $"\"{file}\" \"{key}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                try
                {
                    using var process = Process.Start(psi);
                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();
                    process.WaitForExit();

                    switch (process.ExitCode)
                    {
                        case 0:
                            successCount++;
                            break;
                        case -2:
                            System.Windows.MessageBox.Show("CryptoSoft est déjà en cours d'exécution. Traitement interrompu.", "Instance en cours", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        default:
                            errorCount++;
                            errorDetails.Add($"Erreur pour le fichier {Path.GetFileName(file)}: {error}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    errorCount++;
                    errorDetails.Add($"Exception pour le fichier {Path.GetFileName(file)}: {ex.Message}");
                }
            }

            string summary = $"Traitement terminé :\n" +
                              $"{successCount} fichiers chiffrés avec succès.\n" +
                              $"{errorCount} erreurs rencontrées.";

            if (errorCount > 0)
            {
                summary += "\nDétails des erreurs :\n" + string.Join("\n", errorDetails);
            }

            System.Windows.MessageBox.Show(summary, "Résultat du traitement", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}