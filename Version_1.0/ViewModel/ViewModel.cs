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
        private Model.Work _currentWork = new Model.Work();

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
            return output = _currentWork.name;
        }

        public void SetName(string input)
        {
            _currentWork.name = input;
        }

        public string GetSource()
        {
            return output = _currentWork.source;
        }
        public void SetSource(string input)
        {
            _currentWork.source = input;
        }
        public string GetTarget()
        {
            return output = _currentWork.target;
        }
        public void SetTarget(string input)
        {
            _currentWork.target = input;
        }

        public string GetType()
        {
            return output = _currentWork.type;
        }
        public void SetType(string input)
        {
            _currentWork.type = input;
        }

        public string GetState()
        {
            return output = _currentWork.state;
        }

        public string SetState(string input)
        {
            _currentWork.state = input;
            return _currentWork.state;
        }

        // Méthodes pour la gestion de la liste des travaux
        public void AddWork()
        {
            // Créer une copie du travail actuel et l'ajouter à la liste
            Model.Work newWork = new Model.Work
            {
                name = _currentWork.name,
                source = _currentWork.source,
                target = _currentWork.target,
                type = _currentWork.type,
                state = _currentWork.state
            };

            _works.Add(newWork);

            // Réinitialiser le travail actuel
            _currentWork = new Model.Work();
        }

        public List<Model.Work> GetAllWorks()
        {
            return _works;
        }

        public Model.Work GetWorkByName(string name)
        {
            return _works.FirstOrDefault(w => w.name == name);
        }

        public bool DeleteWork(string name)
        {
            Model.Work workToDelete = _works.FirstOrDefault(w => w.name == name);
            if (workToDelete != null)
            {
                return _works.Remove(workToDelete);
            }
            return false;
        }

        public void LoadWorkToCurrent(string name)
        {
            Model.Work work = GetWorkByName(name);
            if (work != null)
            {
                _currentWork.name = work.name;
                _currentWork.source = work.source;
                _currentWork.target = work.target;
                _currentWork.type = work.type;
                _currentWork.state = work.state;
            }
        }

        public void UpdateWork(string name)
        {
            // Trouver le travail dans la liste
            Model.Work workToUpdate = _works.FirstOrDefault(w => w.name == name);
            if (workToUpdate != null)
            {
                // Mettre à jour avec les valeurs du travail actuel
                int index = _works.IndexOf(workToUpdate);
                _works[index] = _currentWork;
            }
        }

        public int GetWorkCount()
        {
            return _works.Count;
        }
    }
}