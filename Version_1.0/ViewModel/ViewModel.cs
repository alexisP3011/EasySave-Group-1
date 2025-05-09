using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version_1._0.ViewModel
{
    public class ViewModel
    {
        // Collection de travaux
        private List<Model.Work> _works = new List<Model.Work>();

        // Travail actuel pour les opérations
        public Model.Work _currentWork = new Model.Work();

        public ViewModel()
        {
            //input = "";
            //output = "";
        }
        public string input { get; set; }
        public string output { get; set; }

        // Méthodes pour le travail actuel
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
            Model.Work newWork = new Model.Work();

            // Set properties of the new work using the current work
            newWork.SetName(_currentWork.GetName());
            newWork.SetSource(_currentWork.GetSource());
            newWork.SetTarget(_currentWork.GetTarget());
            newWork.SetType(_currentWork.GetType());
            newWork.SetState(_currentWork.GetState());

            // Add the new work to the collection
            _works.Add(newWork);

            // Reset the current work
            _currentWork = new Model.Work();
        }

        public List<Model.Work> GetAllWorks()
        {
            return _works;
        }

        public Model.Work GetWorkByName()
        {
            return _works.FirstOrDefault(w => w.GetName() == input);
        }

        public bool DeleteWork()
        {
            Model.Work workToDelete = _works.FirstOrDefault(w => w.GetName() == input);
            if (workToDelete != null)
            {
                return _works.Remove(workToDelete);
            }
            return false;
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

            Model.Work workToUpdate = _works.FirstOrDefault(w => w.GetName() == input);
            if (workToUpdate != null)
            {

                int index = _works.IndexOf(workToUpdate);
                _works[index] = _currentWork;
            }
        }

        public int GetWorkCount()
        {
            return _works.Count;
        }

        public void LaunchWork()
        {
            _currentWork.LaunchWork();
        }
    }
}