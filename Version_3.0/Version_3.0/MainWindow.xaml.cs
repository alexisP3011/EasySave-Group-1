using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Resources;
using Version_3._0.Model;
using Version_3._0.View.PopUp;
using Version_3._0.View.Popup;

namespace Version_3._0
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        WorkStorage storage = WorkStorage.getInstance();
        RealTimeLog realTimeLog = RealTimeLog.getInstance();
        ResourceManager _rm = new ResourceManager("Version_3._0.Ressources.string", typeof(MainWindow).Assembly);
        SettingsPopup settings = new SettingsPopup();
        private Action<double> progressHandler;

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


                if (Works != null)
                {
                    foreach (var work in Works)
                    {
                        work.IsSelected = value;
                    }
                }
            }

        }

        private double progress;
        public double Progress
        {
            get => progress;
            set
            {
                if (progress != value)
                {
                    progress = value;
                    OnPropertyChanged(nameof(Progress));
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            settings.LoadSettings();
            Works = new ObservableCollection<Work>();

            storage.LoadAllWorks();
            foreach (var workEntry in storage.LoadAllWorks())
            {
                Works.Add(new Work
                {
                    Name = workEntry.Name,
                    Source = workEntry.Source,
                    Target = workEntry.Target,
                    Type = workEntry.Type,
                    State = workEntry.State
                });
            }


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
            storage.AddWorkEntry(newWork.Name, newWork.Source, newWork.Target, newWork.Type, newWork.State);
            realTimeLog.CreateLogFile();
            realTimeLog.AddLogEntry(
                newWork.Name,
                newWork.Source,
                newWork.Target,
                newWork.State,
                realTimeLog.TotalFilesToCopy(newWork.Source, newWork.State),
                realTimeLog.TotalFilesSize(newWork.Source, newWork.State),
                realTimeLog.NbFilesLeftToDo(newWork.Source, newWork.Target, newWork.State),
                realTimeLog.Progression(newWork.Source, newWork.Target)
                );
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

                string message = Works.Count > 1 ? string.Format(_rm.GetString("MultipleDeletionConfiramtionMessageAll"), Works.Count) : _rm.GetString("MultipleDeletionConfirmationMessageOne");

                if (MessageBox.Show(message, _rm.GetString("MultipleDeletionConfirmationMessageTitle"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    for (int i = Works.Count - 1; i >= 0; i--)
                    {
                        if (Works[i].IsSelected)
                        {

                            storage.DeleteWorkEntry(Works[i].Name);
                            realTimeLog.DeleteRealTimeLogEntry(Works[i].Name);
                            Works.RemoveAt(i);
                            //storage.LoadAllWorks();


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
                    string message = selectedCount > 1 ? string.Format(_rm.GetString("DeletionConfirmationMessageMultiple"), selectedCount) : string.Format(_rm.GetString("DeletionConfirmationMessageOne"), selectedWorks[0].Name);

                    if (MessageBox.Show(message, _rm.GetString("DeletionConfirmationMessageTitle"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        foreach (var work in selectedWorks)
                        {
                            storage.DeleteWorkEntry(work.Name);
                            realTimeLog.DeleteRealTimeLogEntry(work.Name);
                            Works.Remove(work);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(_rm.GetString("ErrorDeletionMessage"), _rm.GetString("InformationMessageTitle"));
                }
            }
        }




        // Classe personnalisée pour contenir les informations de cancellation
        public class WorkCancellationInfo
        {
            public CancellationTokenSource TokenSource { get; set; }
            public string CancellationReason { get; set; } // "stop" ou "pause"

            public WorkCancellationInfo()
            {
                TokenSource = new CancellationTokenSource();
                CancellationReason = "stop"; // Par défaut
            }
        }


        private Dictionary<Work, WorkCancellationInfo> workCancellationInfos = new Dictionary<Work, WorkCancellationInfo>();


        public void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedWorks = Works.Where(w => w.IsSelected).ToList();

            SettingsPopup settingsPopup = new SettingsPopup();
            string software = settingsPopup.Software;
            string targetExtension = settingsPopup.TargetExtension;
            string key = settingsPopup.EncryptionKey;

            if (!string.IsNullOrEmpty(targetExtension) && targetExtension.Trim() != "" && string.IsNullOrWhiteSpace(key))
            {
                MessageBox.Show(_rm.GetString("ErrorCryptoSoft"), _rm.GetString("InformationMessageTitle"), MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


            DailyLog logger = DailyLog.getInstance();
            logger.createLogFile();

            if (Process.GetProcessesByName(software).Length > 0)
            {
                MessageBox.Show(_rm.GetString("ErrorSofwareLaunchMessage"), _rm.GetString("InformationMessageTitle"), MessageBoxButton.OK, MessageBoxImage.Information);

                foreach (var work in selectedWorks)
                {
                    logger.LogSaveError(work.Name, software);
                }

                return;
            }

            if (selectedWorks.Count == 0)
            {
                MessageBox.Show(_rm.GetString("ErrorSelectLaunchMessage"), _rm.GetString("InformationMessageTitle"), MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }



            string confirmationMessage = selectedWorks.Count == 1 ? _rm.GetString("ConfirmationLaunchMessageOne") : string.Format(_rm.GetString("ConfirmationLaunchMessageMultiple"), selectedWorks.Count);

            var result = MessageBox.Show(confirmationMessage, _rm.GetString("ConfirmationLaunchMessageTitle"), MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            var tasks = new List<Task>();

            foreach (var work in selectedWorks)
            {
                if (work.State == "inactive" || work.State == "stop" || work.State == "paused")
                {
                    work.State = "active";



                    var workCopy = work;

                    LaunchRealTimeLog(workCopy);
                    var loggerCopy = logger;

                    var cancellationInfo = new WorkCancellationInfo();
                    workCancellationInfos[workCopy] = cancellationInfo;
                    var token = cancellationInfo.TokenSource.Token;

                    var progressHandler = new Progress<double>(value =>
                    {
                        currentWork.Progress = value;
                    });

                    var priorityExtension = settings.PriorityExtension;
                    var MaxFileSizeTransfert = int.Parse(settings.FileSizeTransfert);

                    var task = Task.Run(() =>
                    {
                        try
                        {
                            loggerCopy.TransferFilesWithLogs(
                                workCopy.Source,
                                workCopy.Target,
                                workCopy.Name,
                                priorityExtension,
                                MaxFileSizeTransfert,
                                token,
                                CheckSoftwareOpen,
                                software,
                                false,
                                progressHandler);

                            Dispatcher.Invoke(() =>
                            {
                                if (workCopy.State != "stop" && workCopy.State != "paused")
                                {
                                    workCopy.State = "finished";

                                    LaunchRealTimeLog(workCopy);

                                    storage.DeleteWorkEntry(workCopy.Name);
                                    storage.AddWorkEntry(workCopy.Name, workCopy.Source, workCopy.Target, workCopy.Type, "finished");
                                }
                            });
                        }
                        catch (OperationCanceledException)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                string newState = cancellationInfo.CancellationReason;
                                workCopy.State = newState;
                                LaunchRealTimeLog(workCopy);

                                storage.DeleteWorkEntry(workCopy.Name);
                                storage.AddWorkEntry(workCopy.Name, workCopy.Source, workCopy.Target, workCopy.Type, newState);
                            });
                        }
                        finally
                        {
                            if (workCancellationInfos.ContainsKey(workCopy))
                            {
                                workCancellationInfos.Remove(workCopy);
                            }
                        }
                    }, token);

                    tasks.Add(task);
                }
            }
        }

        public void StopButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedWorks = Works.Where(w => w.IsSelected && w.State == "active").ToList();

            var inactiveSelected = Works.Where(w => w.IsSelected && (w.State == "inactive" || w.State == "paused")).ToList();
            if (inactiveSelected.Any())
            {
                MessageBox.Show(_rm.GetString("ErrorStopZeroWork"), _rm.GetString("InformationMessageTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (selectedWorks.Count == 0)
            {
                MessageBox.Show(_rm.GetString("ErrorStopAnyWork"), _rm.GetString("InformationMessageTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string confirmationMessage = selectedWorks.Count == 1 ? _rm.GetString("ConfirmationStopWork") : string.Format(_rm.GetString("ConfirmationStopMultipleWorks"), selectedWorks.Count);

            var result = MessageBox.Show(confirmationMessage, _rm.GetString("ConfirmationLaunchMessageTitle"), MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            foreach (var work in selectedWorks)
            {
                if (workCancellationInfos.TryGetValue(work, out var cancellationInfo))
                {
                    cancellationInfo.CancellationReason = "stop";
                    cancellationInfo.TokenSource.Cancel();
                }
            }
        }


        public void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedWorks = Works.Where(w => w.IsSelected && w.State == "active").ToList();

            var inactiveSelected = Works.Where(w => w.IsSelected && (w.State == "inactive" || w.State == "paused")).ToList();
            if (inactiveSelected.Any())
            {
                MessageBox.Show(_rm.GetString("ErrorPauseZeroWork"), _rm.GetString("InformationMessageTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (selectedWorks.Count == 0)
            {
                MessageBox.Show(_rm.GetString("ErrorPauseAnyWork"), _rm.GetString("InformationMessageTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string confirmationMessage = selectedWorks.Count == 1 ? _rm.GetString("ConfirmationPauseWork") : string.Format(_rm.GetString("ConfirmationPauseMultipleWorks"), selectedWorks.Count);

            var result = MessageBox.Show(confirmationMessage, _rm.GetString("ConfirmationLaunchMessageTitle"), MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            foreach (var work in selectedWorks)
            {
                if (workCancellationInfos.TryGetValue(work, out var cancellationInfo))
                {
                    cancellationInfo.CancellationReason = "paused";
                    cancellationInfo.TokenSource.Cancel();
                }
            }
        }

        public void CheckSoftwareOpen(string software)
        {

            var workInProgress = Works.Where(w => w.State == "active").ToList();


            if (Process.GetProcessesByName(software).Length > 0)
            {

                foreach (var work in workInProgress)
                {
                    if (workCancellationInfos.TryGetValue(work, out var cancellationInfo))
                    {
                        cancellationInfo.CancellationReason = "paused";
                        cancellationInfo.TokenSource.Cancel();
                    }
                }
            }


        }

        public void LaunchRealTimeLog(Work workToUpdate)
        {

            var allLogs = realTimeLog.LoadRealTimeLog();

            int indexToUpdate = allLogs.FindIndex(l => l.Name == workToUpdate.Name);
            if (indexToUpdate != -1)
            {

                allLogs[indexToUpdate].State = workToUpdate.State;

                allLogs[indexToUpdate].NbFilesLeftToDo = realTimeLog.NbFilesLeftToDo(workToUpdate.Source, workToUpdate.Target, workToUpdate.State);
                allLogs[indexToUpdate].Progression = realTimeLog.Progression(workToUpdate.Source, workToUpdate.Target);

                realTimeLog.DeleteRealTimeLog();

                foreach (var log in allLogs)
                {
                    realTimeLog.SaveEntry(log);
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

        private void Button_Click_Update(object sender, RoutedEventArgs e)
        {

            var selectedWorks = Works.Where(w => w.IsSelected).ToList();

            if (selectedWorks.Count == 0)
            {
                MessageBox.Show(_rm.GetString("ErrorNoSelectUpdateMessage"), _rm.GetString("InformationMessageTitle"), MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (selectedWorks.Count > 1)
            {
                MessageBox.Show(_rm.GetString("ErrorMultipleSelectUpdateMessage"), _rm.GetString("InformationMessageTitle"), MessageBoxButton.OK, MessageBoxImage.Information);
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

            int index = Works.IndexOf(originalWork);

            if (index != -1)
            {

                Works[index].Source = updatedWork.Source;
                Works[index].Target = updatedWork.Target;
                Works[index].Type = updatedWork.Type;
                //Works[index].State = "finished";
                storage.DeleteWorkEntry(updatedWork.Name);
                //storage.AddWorkEntry(updatedWork.Name, updatedWork.Source, updatedWork.Target, updatedWork.Type, "Finished");

                MessageBox.Show(string.Format(_rm.GetString("SuccessMessageUpdate"), updatedWork.Name), _rm.GetString("SuccessMessageTitle"), MessageBoxButton.OK, MessageBoxImage.Information);
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
        private double progress;

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

        public double Progress
        {
            get => progress;
            set
            {
                progress = value;
                OnPropertyChanged(nameof(Progress));
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