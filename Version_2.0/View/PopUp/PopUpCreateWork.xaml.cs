using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
namespace Version_2._0.View.PopUp
{
    /// <summary>
    /// Logique d'interaction pour PopUpCreateWork.xaml
    /// </summary>
    public partial class PopUpCreateWork : Window
    {
        public delegate void WorkCreatedEventHandler(Work newWork);
        public event WorkCreatedEventHandler WorkCreated;
        public PopUpCreateWork()
        {
            InitializeComponent();
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Valider les entrées
            if (string.IsNullOrWhiteSpace(JobNameTextBox.Text))
            {
                ErrorMessageTextBlock.Text = "Please enter a name for the job.";
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }
            if (string.IsNullOrWhiteSpace(SourcePathTextBox.Text))
            {
                ErrorMessageTextBlock.Text = "Please select a source folder.";
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }
            if (string.IsNullOrWhiteSpace(TargetPathTextBox.Text))
            {
                ErrorMessageTextBlock.Text = "Please select a target folder.";
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
            MessageBox.Show("The work " + newWork.Name +  " has been created successfully." , "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            
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