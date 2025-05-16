
using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;
using Version_2._0.View.PopUp;
using System.Windows.Controls;

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

        public MainWindow()
        {
            InitializeComponent();

        
            Works = new ObservableCollection<Work>();
            Works.Add(new Work { Name = "Work 1", Source = "Source 1", Target = "Target 1", Type = "Type 1", State = "inactive" });
            Works.Add(new Work { Name = "Work 2", Source = "Source 2", Target = "Target 2", Type = "Type 2", State = "inactive" });
            Works.Add(new Work { Name = "Work 3", Source = "Source 3", Target = "Target 3", Type = "Type 3", State = "inactive" });
            Works.Add(new Work { Name = "Work 4", Source = "Source 4", Target = "Target 4", Type = "Type 4", State = "inactive" });
            Works.Add(new Work { Name = "Work 5", Source = "Source 5", Target = "Target 5", Type = "Type 5", State = "inactive" });

  
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
                if (MessageBox.Show($"sur ?? '{workToDelete.Name}' ?", //mettre la pop up de confirmation ici 
                    "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Works.Remove(workToDelete);
                }
            }
        }

        public void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Work workToLaunch)
            {
                workToLaunch.State = "active";
                MessageBox.Show($"Le travail '{workToLaunch.Name}' a été lancé.", "Information");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Work : INotifyPropertyChanged
    {
        private string name;
        private string source;
        private string target;
        private string type;
        private string state;

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

        public Work()
        {
            Name = "";
            Source = "";
            Target = "";
            Type = "";
            State = "inactive";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}