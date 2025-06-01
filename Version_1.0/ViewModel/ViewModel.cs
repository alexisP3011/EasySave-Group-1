using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version_1._0.Model;

namespace Version_1._0.ViewModel
{
    public class ViewModel
    {
        // Collection of works
        private List<Model.Work> _works = new List<Model.Work>();

        // Current work for operations
        public Model.Work _currentWork = new Model.Work();

        Model.WorkStorage storage = Model.WorkStorage.getInstance();

        Model.RealTimeLog realTimeLog = Model.RealTimeLog.getInstance();

        Settings settings = Settings.getInstance();

        public ViewModel()
        {
            //input = "";
            //output = "";
        }
        public string input { get; set; }
        public string output { get; set; }

        // Methods for the current work
        public string GetName()
        {
            return output = _currentWork.GetName();
        }

        public void SetName()
        {
            _currentWork.SetName(input);
        }

        public string GetSource()
        {
            return output = _currentWork.GetSource();
        }
        public void SetSource()
        {
            _currentWork.SetSource(input);
        }
        public string GetTarget()
        {
            return output = _currentWork.GetTarget();
        }
        public void SetTarget()
        {
            _currentWork.SetTarget(input);
        }

        public string GetType()
        {
            return output = _currentWork.GetType();
        }
        public void SetType()
        {
            _currentWork.SetType(input);
        }

        public string GetState()
        {
            return output = _currentWork.GetState();
        }

        public void SetState()
        {
            _currentWork.SetState(input);
        }

        public void AddWork()
        {
            // Create a new instance of Model.Work

            storage.AddWorkEntry(
                _currentWork.GetName(),
                _currentWork.GetSource(),
                _currentWork.GetTarget(),
                _currentWork.GetType(),
                _currentWork.GetState()
            );
            realTimeLog.CreateLogFile();
            realTimeLog.AddLogEntry(
                _currentWork.GetName(),
                _currentWork.GetSource(),
                _currentWork.GetTarget(),
                _currentWork.GetState(),
                realTimeLog.TotalFilesToCopy(_currentWork.GetSource(), _currentWork.GetState()),
                realTimeLog.TotalFilesSize(_currentWork.GetSource(), _currentWork.GetState()),
                realTimeLog.NbFilesLeftToDo(_currentWork.GetSource(), _currentWork.GetTarget(), _currentWork.GetState()),
                realTimeLog.Progression(_currentWork.GetSource(), _currentWork.GetTarget())
                );
        }

        public List<Model.Work> GetAllWorks()
        {
            return storage.LoadAllWorks();
        }

        public Model.Work GetWorkByName()
        {
            var allWorks = storage.LoadAllWorks();
            return allWorks.FirstOrDefault(w => w.GetName() == input);
        }

        public void DeleteWork()
        {
            var allWorks = storage.LoadAllWorks();
            var workToDelete = allWorks.FirstOrDefault(w => w.GetName() == input);
            if (workToDelete != null)
            {
                storage.DeleteWorkEntry(workToDelete.GetName());
            }

            realTimeLog.DeleteRealTimeLogEntry(workToDelete.GetName());

        }

        public void LoadWorkToCurrent()
        {
            Model.Work work = GetWorkByName();
            if (work != null)
            {
                _currentWork.SetName(work.GetName());
                _currentWork.SetSource(work.GetSource());
                _currentWork.SetTarget(work.GetTarget());
                _currentWork.SetType(work.GetType());
                _currentWork.SetState(work.GetState());
            }
        }

        public void UpdateWork()
        {
            // Load all works from storage
            var allWorks = storage.LoadAllWorks();
            // Find the index of the work to update by its name
            int indexToUpdate = allWorks.FindIndex(w => w.GetName() == input);
            if (indexToUpdate != -1)
            {
                // Update the properties of the found object with those of _currentWork
                allWorks[indexToUpdate].SetName(_currentWork.GetName());
                allWorks[indexToUpdate].SetSource(_currentWork.GetSource());
                allWorks[indexToUpdate].SetTarget(_currentWork.GetTarget());
                allWorks[indexToUpdate].SetType(_currentWork.GetType());
                allWorks[indexToUpdate].SetState(_currentWork.GetState());

                // Delete all works from storage
                foreach (var work in storage.LoadAllWorks())
                {
                    storage.DeleteWorkEntry(work.GetName());
                }

                // Reinsert all works in order, including the updated work
                foreach (var work in allWorks)
                {
                    storage.AddWorkEntry(
                        work.GetName(),
                        work.GetSource(),
                        work.GetTarget(),
                        work.GetType(),
                        work.GetState()
                    );
                }
            }
            // Update the real-time log
            UpdateRealTimeLog();
        }

        public int GetWorkCount()
        {
            return storage.AllWorkCount();
        }

        public void LaunchWork()
        {
            _currentWork.LaunchWork();
            UpdateWork();
            UpdateRealTimeLog();
        }

        public void changeLanguage()
        {
            settings.setCulture(input);
        }

        public void UpdateRealTimeLog()
        {
            var allLogs = realTimeLog.LoadRealTimeLog();

            int indexToUpdate = allLogs.FindIndex(l => l.Name == input);
            if (indexToUpdate != -1)
            {
                allLogs[indexToUpdate].Name = _currentWork.GetName();
                allLogs[indexToUpdate].Source = _currentWork.GetSource();
                allLogs[indexToUpdate].Target = _currentWork.GetTarget();
                allLogs[indexToUpdate].State = _currentWork.GetState();
                allLogs[indexToUpdate].TotalFilesToCopy = realTimeLog.TotalFilesToCopy(_currentWork.GetSource(), _currentWork.GetState());
                allLogs[indexToUpdate].TotalFilesSize = realTimeLog.TotalFilesSize(_currentWork.GetSource(), _currentWork.GetState());
                allLogs[indexToUpdate].NbFilesLeftToDo = realTimeLog.NbFilesLeftToDo(_currentWork.GetSource(), _currentWork.GetTarget(), _currentWork.GetState());
                allLogs[indexToUpdate].Progression = realTimeLog.Progression(_currentWork.GetSource(), _currentWork.GetTarget());

                realTimeLog.DeleteRealTimeLog();

                foreach (var log in allLogs)
                {
                    realTimeLog.SaveEntry(log);
                }
            }
        }

        public string setLanguage()
        {
            return settings.LoadSettingsLanguage();
        }

        // ------------------------
        // Ajout du support logFormat
        // ------------------------

        public void SetLogFormat()
        {
            settings.SaveLogFormat(input);
        }

        public string GetLogFormat()
        {
            return settings.LoadLogFormat();
        }

        public void LaunchWorkWithFormat(string format)
        {
            var log = DailyLog.getInstance();
            log.TransferFilesWithLogs(
                _currentWork.GetSource(),
                _currentWork.GetTarget(),
                _currentWork.GetName(),
                _currentWork.GetType() == "1", // booléen : true = complet, false = différentiel
                format.ToUpper() // "XML" ou "JSON"
            );
            UpdateWork();
            UpdateRealTimeLog();
        }
    }
}