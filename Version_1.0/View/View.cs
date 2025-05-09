using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version_1._0.Model;

namespace Version_1._0.View
{
    public class View
    {
        private readonly ViewModel.ViewModel _vm;
        public string language;

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
            if (language == "English")
            {
                Console.WriteLine("Welcome to the application!");
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Create a backup work");
                Console.WriteLine("2. Show all the backup works");
                Console.WriteLine("3. Delete a backup work");
                Console.WriteLine("4. Update a backup work");
                Console.WriteLine("5. Launch a backup work");
                Console.WriteLine("6. Change the application language");
                Console.WriteLine("7. Exit");

                Console.Write("Enter your choice: ");
            }
            else if (language == "French")
            {
                Console.WriteLine("Bienvenue dans l'application !");
                Console.WriteLine("Veuillez sélectionner une option :");
                Console.WriteLine("1. Créer un travail de sauvegarde");
                Console.WriteLine("2. Afficher tous les travaux de sauvegarde");
                Console.WriteLine("3. Supprimer un travail de sauvegarde");
                Console.WriteLine("4. Mettre à jour un travail de sauvegarde");
                Console.WriteLine("5. Lancer un travail de sauvegarde");
                Console.WriteLine("6. Changer la langue de l'application");
                Console.WriteLine("7. Quitter");

                Console.Write("Entrez votre choix : ");
            }

        }

        public void ShowMessageChoice()
        {
            Console.WriteLine();
            if (language == "English")
            {
                Console.WriteLine("Please select an option:");
            }
            else if (language == "French")
            {
                Console.WriteLine("Veuillez sélectionner une option :");
            }
        }

        public void AskConfirmation()
        {
            Console.WriteLine();
            if (language == "English")
            {
                Console.WriteLine("Are you sure you want to delete this work? (y/n)");
            }
            else if (language == "French")
            {
                Console.WriteLine("Êtes-vous sûr de vouloir supprimer ce travail ? (o/n)");
            }
        }

        public void WarnInputError()
        {
            if (language == "English")
            {
                Console.WriteLine("Invalid input. Please try again.");
            }
            else if (language == "French")
            {
                Console.WriteLine("Entrée invalide. Veuillez réessayer.");
            }
        }

        public void ShowList(ViewModel.ViewModel viewModel)
        {
            var works = viewModel.GetAllWorks();
            if (language == "English")
            {
                if (works.Count == 0)
                {
                    Console.WriteLine("No backup works available.");
                }
                else
                {
                    Console.WriteLine("Here is the list of your backup works: ");
                    int index = 1;
                    foreach (var work in works)
                    {
                        Console.WriteLine($"--- Work {index} ---");
                        Console.WriteLine($"Name: {work.name}");
                        Console.WriteLine($"Source: {work.source}");
                        Console.WriteLine($"Target: {work.target}");
                        Console.WriteLine($"Type: {work.type}");
                        Console.WriteLine($"State: {work.state}");
                        Console.WriteLine();
                        index++;
                    }
                }
            }
            else if (language == "French")
            {
                if (works.Count == 0)
                {
                    Console.WriteLine("Aucun travail de sauvegarde disponible.");
                }
                else
                {
                    Console.WriteLine("Voici la liste des travaux de sauvegarde : ");
                    int index = 1;
                    foreach (var work in works)
                    {
                        Console.WriteLine($"--- Travail n°{index} ---");
                        Console.WriteLine($"Nom : {work.name}");
                        Console.WriteLine($"Source : {work.source}");
                        Console.WriteLine($"Cible : {work.target}");
                        Console.WriteLine($"Type : {work.type}");
                        Console.WriteLine($"État : {work.state}");
                        Console.WriteLine();
                        index++;
                    }
                }
            }

            ShowNext();
            Console.ReadKey();
        }

        public void ShowWork(Model.Work work)
        {
            Console.WriteLine();
            if (language == "English")
            {
                Console.WriteLine("Here is the work details: ");
                Console.WriteLine($"Name: {work.name}");
                Console.WriteLine($"Source: {work.source}");
                Console.WriteLine($"Target: {work.target}");
                Console.WriteLine($"Type: {work.type}");
                Console.WriteLine($"State: {work.state}");
            }
            else if (language == "French")
            {
                Console.WriteLine("Voici les détails du travail : ");
                Console.WriteLine($"Nom : {work.name}");
                Console.WriteLine($"Source : {work.source}");
                Console.WriteLine($"Cible : {work.target}");
                Console.WriteLine($"Type : {work.type}");
                Console.WriteLine($"État : {work.state}");
            }
        }

        public void ShowCurrentWork(ViewModel.ViewModel viewModel)
        {
            Console.WriteLine();
            if (language == "English")
            {
                Console.WriteLine("Current work details:");
                Console.WriteLine($"Name: {viewModel.GetName()}");
                Console.WriteLine($"Source: {viewModel.GetSource()}");
                Console.WriteLine($"Target: {viewModel.GetTarget()}");
                Console.WriteLine($"Type: {viewModel.GetType()}");
                Console.WriteLine($"State: {viewModel.GetState()}");
            }
            else if (language == "French")
            {
                Console.WriteLine("Détails du travail en cours :");
                Console.WriteLine($"Nom : {viewModel.GetName()}");
                Console.WriteLine($"Source : {viewModel.GetSource()}");
                Console.WriteLine($"Cible : {viewModel.GetTarget()}");
                Console.WriteLine($"Type : {viewModel.GetType()}");
                Console.WriteLine($"État : {viewModel.GetState()}");
            }
        }

        public string AskWorkName()
        {
            Console.WriteLine();
            if (language == "English")
            {
                Console.WriteLine("Please enter the name of the work:");
            }
            else if (language == "French")
            {
                Console.WriteLine("Veuillez entrer le nom du travail :");
            }
            return Console.ReadLine();
        }

        public void ShowLeaveMessage()
        {
            if (language == "English")
            {
                Console.WriteLine("Thank you for using the application. Goodbye!");
            }
            else if (language == "French")
            {
                Console.WriteLine("Merci d'avoir utilisé notre application. Au revoir !");
            }
            
        }

        public void ShowWork()
        {
            Console.WriteLine();
            if (language == "English")
            {
                Console.WriteLine("Here is the work you selected: ");
            }
            else if (language == "French")
            {
                Console.WriteLine("Voici le travail que vous avez sélectionné : ");
            }
        }

        public void ShowNoWorkAvailable()
        {
            if (language == "English")
            {
                Console.WriteLine("No work available with this name.");
            }
            else if (language == "French")
            {
                Console.WriteLine("Aucun travail disponible avec ce nom.");
            }
            ShowNext();
        }

        public void ShowWorkCreatedMessage()
        {
            Console.WriteLine();
            if (language == "English")
            {
                Console.WriteLine("Work created successfully!");
            }
            else if (language == "French")
            {
                Console.WriteLine("Travail créé avec succès !");
            }
            ShowNext();
        }

        public void ShowWorkDeletedMessage()
        {
            Console.WriteLine();
            if (language == "English")
            {
                Console.WriteLine("Work deleted successfully!");
            }
            else if (language == "French")
            {
                Console.WriteLine("Travail supprimé avec succès !");
            }
            ShowNext();
        }

        public void ShowWorkUpdatedMessage()
        {
            Console.WriteLine();
            if (language == "English")
            {
                Console.WriteLine("Work updated successfully!");
            }
            else if (language == "French")
            {
                Console.WriteLine("Travail mis à jour avec succès !");
            }
            ShowNext();
        }
        public void ShowNext()
        {
            Console.WriteLine();
            if (language == "English")
            {
                Console.WriteLine("Press any key to continue...");
            }
            else if (language == "French")
            {
                Console.WriteLine("Appuyez sur une touche pour continuer...");
            }
            Console.ReadKey();
        }

        public void WarnMaxBackup()
        {
            Console.WriteLine();
            if (language == "English")
            {
                Console.WriteLine("You have reached the maximum number of backup works (5). Please delete one before creating a new one.");
            }
            else if (language == "French")
            {
                Console.WriteLine("Vous avez atteint le nombre maximum de travaux de sauvegarde (5). Veuillez en supprimer un avant d'en créer un nouveau.");
            }
            Console.ReadKey();
            Console.Clear();
        }
        public void WarnLaunch()
        {
            Console.WriteLine();
            if (language == "English")
            {
                Console.WriteLine("Launching the backup work...");
            }
            else if (language == "French")
            {
                Console.WriteLine("Lancement du travail de sauvegarde... ");
            }
            ShowNext();
        }

        public void WarnInvalidOption()
        {
            Console.WriteLine();
            if (language == "English")
            {
                Console.WriteLine("Invalid option. Please try again.");
            }
            else if (language == "French")
            {
                Console.WriteLine("Option invalide. Veuillez réessayer.");
            }
            ShowNext();
        }
    }