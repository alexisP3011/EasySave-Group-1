using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Version_2._0
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Work> _works;
        private ObservableCollection<Work> _filteredWorks;
        private bool _areAllWorksSelected;
        private string _searchText = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Work> Works
        {
            get { return _works; }
            set
            {
                _works = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Work> FilteredWorks
        {
            get { return _filteredWorks; }
            set
            {
                _filteredWorks = value;
                OnPropertyChanged();
            }
        }

        public bool AreAllWorksSelected
        {
            get { return _areAllWorksSelected; }
            set
            {
                if (_areAllWorksSelected != value)
                {
                    _areAllWorksSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Exemple de données
            Works = new ObservableCollection<Work>
            {
                new Work { Name = "Backup 1", Source = "C:\\Documents", Target = "D:\\Backup", Type = "Full", State = "Inactive", IsSelected = false },
                new Work { Name = "Backup 2", Source = "C:\\Photos", Target = "D:\\Backup", Type = "Differential", State = "Inactive", IsSelected = false },
                // Ajoutez plus d'éléments selon vos besoins
            };

            // Initialiser la liste filtrée avec tous les éléments
            FilteredWorks = new ObservableCollection<Work>(Works);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Gestionnaire d'événements pour les boutons
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Logique pour le bouton Create
        }

        private void Button_Click_Update(object sender, RoutedEventArgs e)
        {
            // Logique pour le bouton Update
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Logique pour le bouton Delete
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            // Logique pour le bouton Launch
        }

        private void Button_Settings(object sender, RoutedEventArgs e)
        {
            // Logique pour le bouton Settings
        }

        private void List_Works_Checked(object sender, RoutedEventArgs e)
        {
            AreAllWorksSelected = true;
            foreach (var work in FilteredWorks)
            {
                work.IsSelected = true;
            }
        }

        private void List_Works_Unchecked(object sender, RoutedEventArgs e)
        {
            AreAllWorksSelected = false;
            foreach (var work in FilteredWorks)
            {
                work.IsSelected = false;
            }
        }

        private void WorkCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAllWorkSelectionStatus();
        }

        private void WorkCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            AreAllWorksSelected = false;
        }

        private void UpdateAllWorkSelectionStatus()
        {
            AreAllWorksSelected = FilteredWorks.All(w => w.IsSelected);
        }

        // Nouveaux gestionnaires pour la recherche
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _searchText = SearchBox.Text.ToLower();
            ApplyFilter();
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Clear();
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(_searchText))
            {
                // Si la recherche est vide, afficher tous les éléments
                FilteredWorks = new ObservableCollection<Work>(Works);
            }
            else
            {
                // Filtrer les éléments selon le texte de recherche
                var filtered = Works.Where(w =>
                    w.Name.ToLower().Contains(_searchText) ||
                    w.Source.ToLower().Contains(_searchText) ||
                    w.Target.ToLower().Contains(_searchText) ||
                    w.Type.ToLower().Contains(_searchText) ||
                    w.State.ToLower().Contains(_searchText)
                ).ToList();

                FilteredWorks = new ObservableCollection<Work>(filtered);
            }

            // Mettre à jour l'état de la case à cocher "Tous sélectionner"
            UpdateAllWorkSelectionStatus();
        }
    }

    public class Work : INotifyPropertyChanged
    {
        private bool _isSelected;

        public string Name { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string Type { get; set; }
        public string State { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}