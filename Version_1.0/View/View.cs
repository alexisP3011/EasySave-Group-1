using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Version_1._0.Model;
using System.Threading;

namespace Version_1._0.View
{
    public class View
    {
        private readonly ViewModel.ViewModel _vm;
        protected readonly ResourceManager _rm = new ResourceManager("Version_1._0.Ressources.string", typeof(View).Assembly);

        public View()
        {
            _vm = new ViewModel.ViewModel();
        }

        public View(ViewModel.ViewModel viewModel)
        {
            _vm = viewModel;
        }
        
        
        public void ShowMenu()
        {
            Console.WriteLine(_rm.GetString("WelcomeMessage"));
            Console.WriteLine(_rm.GetString("SelectOption"));
            Console.WriteLine(_rm.GetString("MenuChoices"));
            Console.Write(_rm.GetString("EnterChoice"));
        }

        public void ShowMessageChoice()
        {
            Console.WriteLine();
            Console.WriteLine(_rm.GetString("SelectOption"));
        }

        public void AskConfirmation()
        {
            Console.WriteLine();
            Console.WriteLine(_rm.GetString("Confirmation"));
        }

        public void WarnInputError()
        {
            Console.WriteLine(_rm.GetString("InputError"));
        }

        public void ShowList(ViewModel.ViewModel viewModel)
        {
            var works = viewModel.GetAllWorks();
            if (works.Count == 0)
            {
                Console.WriteLine(_rm.GetString("NoBackupAvailable"));
            }
            else
            {
                Console.WriteLine(_rm.GetString("List"));
                int index = 1;
                foreach (var work in works)
                {
                    Console.WriteLine(_rm.GetString("Works"), index, work.GetName(), work.GetSource(), work.GetTarget(), work.GetType(), work.GetState());
                    Console.WriteLine();
                    index++;
                }
            }
            ShowNext();
        }

        public void ShowWork(Model.Work work)
        {
            Console.WriteLine();
            Console.WriteLine(_rm.GetString("Work"), work.GetName(), work.GetSource(), work.GetTarget(), work.GetType(), work.GetState());
        }

        public void ShowCurrentWork(ViewModel.ViewModel viewModel)
        {
            Console.WriteLine();
            Console.WriteLine(_rm.GetString("CurrentWork"), viewModel.GetName(), viewModel.GetSource(), viewModel.GetTarget(), viewModel.GetType(), viewModel.GetState());
        }

        public string AskWorkName()
        {
            Console.WriteLine();
            Console.WriteLine(_rm.GetString("AskWorkName"));
            return Console.ReadLine();
        }

        public void ShowLeaveMessage()
        {
            Console.WriteLine(_rm.GetString("LeaveMessage"));
        }

        public void ShowNoWorkAvailable()
        {
            Console.WriteLine(_rm.GetString("NoWorkAvailable"));
            ShowNext();
        }

        public void ShowWorkCreatedMessage()
        {
            Console.WriteLine();
            Console.WriteLine(_rm.GetString("CreateWorkMessage"));
            ShowNext();
        }

        public void ShowWorkDeletedMessage()
        {
            Console.WriteLine();
            Console.WriteLine(_rm.GetString("DeleteWorkMessage"));
            ShowNext();
        }

        public void ShowWorkUpdatedMessage()
        {
            Console.WriteLine();
            Console.WriteLine(_rm.GetString("UpdateWorkMessage"));
            ShowNext();
        }
        public void ShowNext()
        {
            Console.WriteLine();
            Console.WriteLine(_rm.GetString("ContinueMessage"));
            Console.ReadKey();
        }

        public void WarnMaxBackup()
        {
            Console.WriteLine();
            Console.WriteLine(_rm.GetString("MaxBackupMessage"));
            Console.ReadKey();
            Console.Clear();
        }
        public void WarnLaunch()
        {
            Console.WriteLine();
            Console.WriteLine(_rm.GetString("LaunchMessage"));
            ShowNext();
        }

        public void WarnInvalidOption()
        {
            Console.WriteLine();
            Console.WriteLine(_rm.GetString("InvalidOption"));
            ShowNext();
        }
    }
}