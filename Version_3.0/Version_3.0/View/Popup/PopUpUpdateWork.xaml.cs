using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using Microsoft.Win32;
using System.IO;
using System.Resources;
using Version_3._0.View.PopUp;

namespace Version_3._0.View.PopUp
{
    /// <summary>
    /// Logique d'interaction pour PopUpUpdateWork.xaml
    /// </summary>
    public partial class PopUpUpdateWork : Window
    {
        private Work _originalWork;
        RealTimeLog realTimeLog = RealTimeLog.getInstance();
        private ResourceManager _rm = new ResourceManager("Version_3._0.Ressources.string", typeof(PopUpUpdateWork).Assembly);

        public delegate void WorkUpdatedEventHandler(Work originalWork, Work updatedWork);
        public event WorkUpdatedEventHandler WorkUpdated;

        public PopUpUpdateWork(Work workToUpdate)
        {
            InitializeComponent();

            _originalWork = workToUpdate;

            JobNameTextBox.Text = workToUpdate.Name;
            SourcePathTextBox.Text = workToUpdate.Source;
            TargetPathTextBox.Text = workToUpdate.Target;


            foreach (ComboBoxItem item in JobTypeComboBox.Items)
            {
                if (item.Content.ToString() == workToUpdate.Type)
                {
                    item.IsSelected = true;
                    break;
                }
            }


            if (ConfirmButton.Template.FindName("ConfirmButtonText", ConfirmButton) is TextBlock textBlock)
            {
                textBlock.Text = "Update";
            }

            UpdateWindow.Title = _rm.GetString("UpdatePopUpTitle");
            UpdateTitle.Text = _rm.GetString("UpdateWorkPopUp");
            NameLabel.Text = _rm.GetString("NameLabel");
            SourceLabel.Text = _rm.GetString("SourceLabel");
            TargetLabel.Text = _rm.GetString("TargetLabel");
            TypeLabel.Text = _rm.GetString("TypeLabel");
            ConfirmButton.ApplyTemplate();
            TextBlock confirmTextBlock = ConfirmButton.Template.FindName("ConfirmButtonText", ConfirmButton) as TextBlock;
            confirmTextBlock.Text = _rm.GetString("ConfirmButton");
            CancelButton.ApplyTemplate();
            TextBlock cancelTextBlock = CancelButton.Template.FindName("CancelButtonText", CancelButton) as TextBlock;
            cancelTextBlock.Text = _rm.GetString("CancelButton");
        }

        public PopUpUpdateWork()
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



        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(JobNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(SourcePathTextBox.Text) ||
                string.IsNullOrWhiteSpace(TargetPathTextBox.Text))
            {
                ErrorMessageTextBlock.Text = _rm.GetString("ErrorRequiredFieldsUpdate");

                ErrorMessageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            //// Check if the source folder exists
            //if (!Directory.Exists(SourcePathTextBox.Text))
            //{
            //    ErrorMessageTextBlock.Text = "Doesn't exist";
            //    ErrorMessageTextBlock.Visibility = Visibility.Visible;
            //    return;
            //}


            var updatedWork = new Work
            {
                Name = JobNameTextBox.Text,
                Source = SourcePathTextBox.Text,
                Target = TargetPathTextBox.Text,
                Type = (JobTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Complete",
                State = _originalWork?.State ?? "inactive",
                IsSelected = _originalWork?.IsSelected ?? false
            };

            // Trigger the event to notify that the work has been updated
            WorkUpdated?.Invoke(_originalWork, updatedWork);
            UpdateRealTimeLog(updatedWork);

            // Close the window
            this.Close();
        }



        public void UpdateRealTimeLog(Work workToUpdate)
        {

            var allLogs = realTimeLog.LoadRealTimeLog();

            int indexToUpdate = allLogs.FindIndex(l => l.Name == workToUpdate.Name);
            if (indexToUpdate != -1)
            {
                allLogs[indexToUpdate].Name = workToUpdate.Name;
                allLogs[indexToUpdate].Source = workToUpdate.Source;
                allLogs[indexToUpdate].Target = workToUpdate.Target;
                allLogs[indexToUpdate].State = workToUpdate.State;
                allLogs[indexToUpdate].TotalFilesToCopy = realTimeLog.TotalFilesToCopy(workToUpdate.Source, workToUpdate.Target);
                allLogs[indexToUpdate].TotalFilesSize = realTimeLog.TotalFilesSize(workToUpdate.Source, workToUpdate.Target);
                allLogs[indexToUpdate].NbFilesLeftToDo = realTimeLog.NbFilesLeftToDo(workToUpdate.Source, workToUpdate.Target, workToUpdate.State);
                allLogs[indexToUpdate].Progression = realTimeLog.Progression(workToUpdate.Source, workToUpdate.Target);

                realTimeLog.DeleteRealTimeLog();

                foreach (var log in allLogs)
                {
                    realTimeLog.SaveEntry(log);
                }
            }

        }
    }
}
