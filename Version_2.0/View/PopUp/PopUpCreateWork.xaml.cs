using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            string jobName = JobNameTextBox.Text;
            string sourcePath = SourcePathTextBox.Text;
            string targetPath = TargetPathTextBox.Text;

            string jobType = null;
            if (JobTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                jobType = selectedItem.Content.ToString();
            }


            Work work = new Work
            {
                Name = jobName,
                Source = sourcePath,
                Target = targetPath,
                Type = jobType
            };


            WorkCreated?.Invoke(work);

            this.Close();
        }
    }
}