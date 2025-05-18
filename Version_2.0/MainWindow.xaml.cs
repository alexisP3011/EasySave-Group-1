using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;
using Version_2._0.View.PopUp;
using System.Windows.Controls;
using System.Linq;
using System.IO;

namespace Version_2._0
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Work> works;
        public ObservableCollection<Work> Works
        {
            get => works;
            set
            {
                works = value;
                OnPropertyChanged(nameof(Works));
            }
        }

        private Work currentWork;
        public Work CurrentWork
        {
            get => currentWork;
            set
            {
                currentWork = value;
                OnPropertyChanged(nameof(CurrentWork));
            }
        }

        private bool _areAllWorksSelected;
        public bool AreAllWorksSelected
        {
            get => _areAllWorksSelected;
            set
            {
                _areAllWorksSelected = value;
                OnPropertyChanged(nameof(AreAllWorksSelected));

                // Mettre à jour toutes les checkboxes individuelles
                if (Works != null)
                {
                    foreach (var work in Works)
                    {
                        work.IsSelected = value;
                    }
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            Works = new ObservableCollection<Work>();

          
            Works.Add(new Work { Name = "Work 1", Source = "C:\\Users\\pfrsc\\OneDrive - Association Cesi Viacesi mail\\Bus", Target = "C:\\Users\\pfrsc\\OneDrive - Association Cesi Viacesi mail\\Python", Type = "Type 1", State = "inactive" });
            Works.Add(new Work { Name = "Work 2", Source = "Source 2", Target = "Target 2", Type = "Type 2", State = "inactive" });
            Works.Add(new Work { Name = "Work 3", Source = "Source 3", Target = "Target 3", Type = "Type 3", State = "inactive" });
            Works.Add(new Work { Name = "Work 4", Source = "Source 4", Target = "Target 4", Type = "Type 4", State = "inactive" });

            if (Works.Count > 0)
                CurrentWork = Works[0];

            this.DataContext = this;
        }

        public void OpenCreateWorkPopUp()
        {
            PopUpCreateWork popup = new PopUpCreateWork();
            popup.WorkCreated += OnWorkCreated;
            popup.Owner = this;
            popup.ShowDialog();
        }

        private void OnWorkCreated(Work newWork)
        {
            Works.Add(newWork);
            CurrentWork = newWork;
        }

        public void CreateWorkButton_Click(object sender, RoutedEventArgs e)
        {
            OpenCreateWorkPopUp();
        }

        public void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Work workToUpdate)
            {
                workToUpdate.State = workToUpdate.State == "active" ? "inactive" : "active";
            }
        }

        public void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            if (sender is Button button && button.Tag is Work workToDelete)
            {
                if (MessageBox.Show($"Are you sure you want to delete '{workToDelete.Name}'?", // confirmation pop-up here
                    "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Works.Remove(workToDelete);
                }
            }

            else if (AreAllWorksSelected)
            {

                string message = Works.Count > 1
                    ? $"Are you sure you want to delete all {Works.Count} selected works?"
                    : "Are you sure you want to delete the selected work?";

                if (MessageBox.Show(message, "Multiple Deletion Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    for (int i = Works.Count - 1; i >= 0; i--)
                    {
                        if (Works[i].IsSelected)
                        {
                            Works.RemoveAt(i);
                        }
                    }


                    AreAllWorksSelected = false;
                }
            }

            else
            {

                var selectedWorks = Works.Where(w => w.IsSelected).ToList();
                int selectedCount = selectedWorks.Count;

                if (selectedCount > 0)
                {
                    string message = selectedCount > 1
                        ? $"Are you sure you want to delete the {selectedCount} selected works?"
                        : $"Are you sure you want to delete the work '{selectedWorks[0].Name}'?";

                    if (MessageBox.Show(message, "Deletion Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {

                        for (int i = Works.Count - 1; i >= 0; i--)
                        {
                            if (Works[i].IsSelected)
                            {
                                Works.RemoveAt(i);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No work is selected for deletion.", "Information");
                }
            }
        }

        public void LaunchButton_Click(object sender, RoutedEventArgs e)
        {

            if (sender is Button button && button.Tag is Work workToLaunch)
            {
                workToLaunch.State = "active";
                MessageBox.Show($"Le travail '{workToLaunch.Name}' a été lancé.", "Information");

                if (workToLaunch.State == "inactive")
                {
                    workToLaunch.State = "active";

                    // Verify if the source directory exists
                    if (Directory.Exists(workToLaunch.Source))
                    {
                        // Create the target directory if it doesn't exist
                        if (!Directory.Exists(workToLaunch.Target))
                        {
                            Directory.CreateDirectory(workToLaunch.Target);
                        }

                        // Copy all of the files to the new location
                        foreach (var file in Directory.GetFiles(workToLaunch.Source))
                        {
                            string fileName = Path.GetFileName(file);
                            string destFile = Path.Combine(workToLaunch.Target, fileName);
                            File.Copy(file, destFile, overwrite: true);
                        }
                    }
                    workToLaunch.State = "finished";

                    //_works.Remove(workToLaunch);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void List_Works_Checked(object sender, RoutedEventArgs e)
        {
            AreAllWorksSelected = true;
        }

        private void List_Works_Unchecked(object sender, RoutedEventArgs e)
        {
            AreAllWorksSelected = false;
        }

        private void WorkCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            // Vérifier si toutes les checkboxes individuelles sont cochées
            UpdateMasterCheckboxState();
        }

        private void WorkCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Si une checkbox individuelle est décochée, décocher la checkbox maître
            List_Works.IsChecked = false;
        }

        private void UpdateMasterCheckboxState()
        {
            bool allChecked = true;
            foreach (var work in Works)
            {
                if (!work.IsSelected)
                {
                    allChecked = false;
                    break;
                }
            }

            // Mettre à jour la checkbox maître sans déclencher l'événement
            if (List_Works.IsChecked != allChecked)
            {
                List_Works.IsChecked = allChecked;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var popup = new PopUpCreateWork();
            popup.WorkCreated += OnWorkCreated;
            popup.Owner = this;
            popup.Show();
        }
    }

    public class Work : INotifyPropertyChanged
    {
        private string name;
        private string source;
        private string target;
        private string type;
        private string state;
        private bool isSelected;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Source
        {
            get => source;
            set
            {
                source = value;
                OnPropertyChanged(nameof(Source));
            }
        }

        public string Target
        {
            get => target;
            set
            {
                target = value;
                OnPropertyChanged(nameof(Target));
            }
        }

        public string Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public string State
        {
            get => state;
            set
            {
                state = value;
                OnPropertyChanged(nameof(State));
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public Work()
        {
            Name = "";
            Source = "";
            Target = "";
            Type = "";
            State = "inactive";
            IsSelected = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}