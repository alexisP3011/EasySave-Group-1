using Microsoft.Win32;
using System;
using System.IO;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using MessageBox = System.Windows.MessageBox;

namespace Version_3._0.View.PopUp
{
    /// <summary>
    /// Interaction logic for PopUpCreateWork.xaml
    /// </summary>
    public partial class PopUpCreateWork : Window
    {
        public delegate void WorkCreatedEventHandler(Work newWork);
        public event WorkCreatedEventHandler WorkCreated;
        private ResourceManager _rm = new ResourceManager("Version_3._0.Ressources.string", typeof(PopUpCreateWork).Assembly);
        public PopUpCreateWork()
        {
            InitializeComponent();
        }

        private void BrowseFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Choose a folder",
                Filter = "Folder|*.none",
                CheckFileExists = false,
                ValidateNames = false,
                FileName = "Select a folder"
            };

            if (dialog.ShowDialog() == true)
            {
                string folder = Path.GetDirectoryName(dialog.FileName);

                if (sender == _3DotSource)
                {
                    SourcePathTextBox.Text = folder;
                }
                else if (sender == _3DotTarget)
                {
                    TargetPathTextBox.Text = folder;
                }
            }

        }


        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Valider les entrées
            if (string.IsNullOrWhiteSpace(JobNameTextBox.Text))
            {
                ErrorMessageTextBlock.Text = _rm.GetString("ErrorMessageTextBlockName");
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }
            if (string.IsNullOrWhiteSpace(SourcePathTextBox.Text))
            {
                ErrorMessageTextBlock.Text = _rm.GetString("ErrorMessageTextBlockSource");
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }
            if (string.IsNullOrWhiteSpace(TargetPathTextBox.Text))
            {
                ErrorMessageTextBlock.Text = _rm.GetString("ErrorMessageTextBlockTarget");
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }
            string jobName = JobNameTextBox.Text;
            string sourcePath = SourcePathTextBox.Text;
            string targetPath = TargetPathTextBox.Text;
            string jobType = "Complete";
            if (JobTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                jobType = selectedItem.Content.ToString();
            }

            Work newWork = new Work
            {
                Name = jobName,
                Source = sourcePath,
                Target = targetPath,
                Type = jobType,
                State = "inactive"
            };

            WorkCreated?.Invoke(newWork);
            string message = string.Format(_rm.GetString("SuccessMessageCreate"), newWork.Name);
            MessageBox.Show(message, _rm.GetString("SuccessMessageTitle"), MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }
        private void ConfirmButton_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
    }
}
