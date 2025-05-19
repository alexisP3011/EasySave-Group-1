using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;
using Version_2._0.View.PopUp;
using System.Windows.Controls;
using System.Linq;
using System.IO;
using Version_2._0.View.Popup;
using System.Diagnostics;

namespace Version_2._0
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        WorkStorage storage = WorkStorage.getInstance();

        // Liste complète des travaux
        private ObservableCollection<Work> _allWorks;

        // Liste filtrée des travaux (pour l'affichage)
        private ObservableCollection<Work> _filteredWorks;
        public ObservableCollection<Work> FilteredWorks
        {
            get => _filteredWorks;
            set
            {
                _filteredWorks = value;
                OnPropertyChanged(nameof(FilteredWorks));
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


                if (FilteredWorks != null)
                {
                    foreach (var work in FilteredWorks)
                    {
                        work.IsSelected = value;
                    }
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            _allWorks = new ObservableCollection<Work>();

            storage.LoadAllWorks();
            foreach (var workEntry in storage.LoadAllWorks())
            {
                _allWorks.Add(new Work
                {
                    Name = workEntry.Name,
                    Source = workEntry.Source,
                    Target = workEntry.Target,
                    Type = workEntry.Type,
                    State = workEntry.State
                });
            }
            //Exemple de données simulées
            //Works.Add(new Work { Name = "Work 1", Source = "C:\\Users\\pfrsc\\OneDrive - Association Cesi Viacesi mail\\Bus", Target = "C:\\Users\\pfrsc\\OneDrive - Association Cesi Viacesi mail\\Python\\end", Type = "Type 1", State = "inactive" });
            //Works.Add(new Work { Name = "Work 5", Source = "C:\\Users\\pfrsc\\OneDrive - Association Cesi Viacesi mail\\Bus", Target = "C:\\Users\\pfrsc\\OneDrive - Association Cesi Viacesi mail\\Python\\end2", Type = "Type 1", State = "inactive" });

            //Works.Add(new Work { Name = "Work 2", Source = "Source 2", Target = "Target 2", Type = "Type 2", State = "inactive" });
            //Works.Add(new Work { Name = "Work 3", Source = "Source 3", Target = "Target 3", Type = "Type 3", State = "inactive" });
            //Works.Add(new Work { Name = "Work 4", Source = "Source 4", Target = "Target 4", Type = "Type 4", State = "inactive" });

            // Initialiser la liste filtrée avec tous les travaux
            FilteredWorks = new ObservableCollection<Work>(_allWorks);

            if (FilteredWorks.Count > 0)
                CurrentWork = FilteredWorks[0];

            this.DataContext = this;
        }

        // Implémentation de l'événement SearchBox_TextChanged
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = SearchBox.Text.ToLower();

            FilteredWorks = new ObservableCollection<Work>(
                _allWorks.Where(w =>
                    w.Name.ToLower().Contains(filter) ||
                    w.Source.ToLower().Contains(filter) ||
                    w.Target.ToLower().Contains(filter) ||
                    w.Type.ToLower().Contains(filter) ||
                    w.State.ToLower().Contains(filter))
            );
        }

        // Implémentation du bouton de réinitialisation de la recherche
        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = string.Empty;
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
            _allWorks.Add(newWork);

            // Rafraîchir la liste filtrée avec le nouveau travail
            FilteredWorks = new ObservableCollection<Work>(_allWorks);

            CurrentWork = newWork;
            storage.AddWorkEntry(newWork.Name, newWork.Source, newWork.Target, newWork.Type, newWork.State);
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
            if (AreAllWorksSelected)
            {
                string message = FilteredWorks.Count > 1
                    ? $"Are you sure you want to delete all {FilteredWorks.Count} selected works?"
                    : "Are you sure you want to delete the selected work?";

                if (MessageBox.Show(message, "Multiple Deletion Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // Créer une liste temporaire des works à supprimer
                    var worksToRemove = FilteredWorks.Where(w => w.IsSelected).ToList();

                    foreach (var work in worksToRemove)
                    {
                        storage.DeleteWorkEntry(work.Name);
                        _allWorks.Remove(work);
                    }

                    // Rafraîchir la liste filtrée
                    FilteredWorks = new ObservableCollection<Work>(_allWorks);

                    AreAllWorksSelected = false;
                }
            }
            else
            {
                var selectedWorks = FilteredWorks.Where(w => w.IsSelected).ToList();
                int selectedCount = selectedWorks.Count;

                if (selectedCount > 0)
                {
                    string message = selectedCount > 1
                        ? $"Are you sure you want to delete the {selectedCount} selected works?"
                        : $"Are you sure you want to delete the work '{selectedWorks[0].Name}'?";

                    if (MessageBox.Show(message, "Deletion Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        foreach (var work in selectedWorks)
                        {
                            storage.DeleteWorkEntry(work.Name);
                            _allWorks.Remove(work);
                        }

                        // Rafraîchir la liste filtrée
                        FilteredWorks = new ObservableCollection<Work>(_allWorks);
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
            var selectedWorks = FilteredWorks.Where(w => w.IsSelected).ToList();

            SettingsPopup settingsPopup = new SettingsPopup();
            string software = settingsPopup.Software;

            DailyLog logger = DailyLog.getInstance();
            logger.createLogFile();

            if (Process.GetProcessesByName(software).Length > 0)
            {
                MessageBox.Show("Please close the software to continue.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                foreach (var work in selectedWorks)
                {
                    logger.LogSaveError(work.Name, software);
                }

                return;
            }

            if (selectedWorks.Count == 0)
            {
                MessageBox.Show("Please select a work to launch.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string confirmationMessage = selectedWorks.Count == 1
                ? "Are you sure you want to launch the selected work?"
                : $"Are you sure you want to launch the {selectedWorks.Count} selected works?";

            var result = MessageBox.Show(confirmationMessage, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            foreach (var work in selectedWorks)
            {
                if (work.State == "inactive")
                {
                    work.State = "active";

                    logger.TransferFilesWithLogs(
                        work.Source,  // Source path
                        work.Target,  // Destination path
                        work.Name);    // Work name

                    work.State = "finished";
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
            UpdateMasterCheckboxState();
        }

        private void WorkCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            List_Works.IsChecked = false;
        }

        private void UpdateMasterCheckboxState()
        {
            bool allChecked = true;
            foreach (var work in FilteredWorks)
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

        private void Button_Click_Update(object sender, RoutedEventArgs e)
        {
            var selectedWorks = FilteredWorks.Where(w => w.IsSelected).ToList();

            if (selectedWorks.Count == 0)
            {
                MessageBox.Show("Please select a work to update.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (selectedWorks.Count > 1)
            {
                MessageBox.Show("Please select only one work to update.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Récupérer le travail sélectionné
            Work workToUpdate = selectedWorks.First();

            // Créer et configurer la popup
            var popup = new PopUpUpdateWork(workToUpdate);
            popup.WorkUpdated += OnWorkUpdated;
            popup.Owner = this;
            popup.ShowDialog();
        }

        private void OnWorkUpdated(Work originalWork, Work updatedWork)
        {
            int index = FilteredWorks.IndexOf(originalWork);

            if (index != -1)
            {
                FilteredWorks[index].Source = updatedWork.Source;
                FilteredWorks[index].Target = updatedWork.Target;
                FilteredWorks[index].Type = updatedWork.Type;

                // Mettre à jour également dans _allWorks
                int allWorksIndex = _allWorks.IndexOf(originalWork);
                if (allWorksIndex != -1)
                {
                    _allWorks[allWorksIndex].Source = updatedWork.Source;
                    _allWorks[allWorksIndex].Target = updatedWork.Target;
                    _allWorks[allWorksIndex].Type = updatedWork.Type;
                }

                storage.DeleteWorkEntry(updatedWork.Name);
                storage.AddWorkEntry(updatedWork.Name, updatedWork.Source, updatedWork.Target, updatedWork.Type, updatedWork.State);

                MessageBox.Show($"The work '{updatedWork.Name}' has been successfully updated.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_Settings(object sender, RoutedEventArgs e)
        {
            var popup = new SettingsPopup();
            popup.Owner = this;
            popup.ShowDialog();
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