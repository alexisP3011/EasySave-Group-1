using System;
using System.ComponentModel;
using System.Diagnostics;
using Version_1._0.Model;
using Version_1._0.View;

namespace Version_1._0
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            var viewModel = new ViewModel.ViewModel();
            var view = new View.View(viewModel);
            var language = new Language();
            view.language = "English";


            while (!exit)
            {
                view.ShowMenu();
                Console.WriteLine("8. Export logs to JSON/XML"); // Ajout de l'option d'export des logs

                ConsoleKeyInfo choice = Console.ReadKey();
                Console.Clear();
                switch (choice.KeyChar)
                {
                    case '1': // Create a new work
                        view.ShowList(viewModel);

                        if (viewModel.GetWorkCount() < 5)
                        {

                            var add = new View.Add();

                            add.language = view.language;

                            add.AskSaveName();
                            viewModel.input = Console.ReadLine();
                            viewModel.SetName();

                            add.AskSource();
                            viewModel.input = Console.ReadLine();
                            viewModel.SetSource();

                            add.AskTarget();
                            viewModel.input = Console.ReadLine();
                            viewModel.SetTarget();

                            add.AskType();
                            viewModel.input = Console.ReadLine();
                            viewModel.SetType();

                            view.AskConfirmation();
                            string confirmation = Console.ReadLine().ToLower();

                            if (confirmation == "y" || confirmation == "yes")
                            {
                                viewModel.AddWork();
                                view.ShowWorkCreatedMessage();
                            }

                            Console.Clear();
                        }
                        else
                        {
                            view.WarnMaxBackup();
                        }
                        break;

                    case '2': // Show work details
                        view.ShowList(viewModel);
                        Console.Clear();
                        break;

                    case '3':  // Delete a work
                        view.ShowList(viewModel);

                        if (viewModel.GetWorkCount() > 0)
                        {
                            string workName = view.AskWorkName();
                            viewModel.input = workName;
                            var workToDelete = viewModel.GetWorkByName();

                            if (workToDelete != null)
                            {
                                view.ShowWork(workToDelete);
                                view.AskConfirmation();
                                string confirmation_3 = Console.ReadLine().ToLower();

                                if (confirmation_3 == "y" || confirmation_3 == "yes")
                                {
                                    viewModel.input = workName;
                                    viewModel.DeleteWork();
                                    view.ShowWorkDeletedMessage();
                                }
                            }
                            else
                            {
                                view.ShowNoWorkAvailable();
                            }
                        }

                        Console.Clear();
                        break;

                    case '4': // Update a work
                        var update = new View.Update();
                        update.language = view.language;

                        update.ShowList(viewModel);

                        if (viewModel.GetWorkCount() > 0)
                        {
                            update.AskSaveName();
                            string updateName = Console.ReadLine();
                            viewModel.input = updateName;
                            var workToUpdate = viewModel.GetWorkByName();
                            if (workToUpdate != null)
                            {
                                viewModel.input = updateName;
                                viewModel.LoadWorkToCurrent();
                                view.ShowCurrentWork(viewModel);

                                update.AskItemToUpdate();
                                ConsoleKeyInfo itemToUpdate = Console.ReadKey();
                                Console.WriteLine();

                                switch (itemToUpdate.KeyChar)
                                {
                                    case '1':
                                        update.AskSaveName();
                                        viewModel.input = Console.ReadLine();
                                        viewModel.SetName();
                                        break;

                                    case '2':
                                        update.AskSource();
                                        viewModel.input = Console.ReadLine();
                                        viewModel.SetSource();
                                        break;
                                    case '3':
                                        update.AskTarget();
                                        viewModel.input = Console.ReadLine();
                                        viewModel.SetTarget();
                                        break;
                                    case '4':
                                        update.AskType();
                                        viewModel.input = Console.ReadLine();
                                        viewModel.SetType();
                                        break;
                                    case '5':
                                        break;
                                    default:
                                        view.WarnInputError();
                                        break;
                                }

                                if (itemToUpdate.KeyChar >= '1' && itemToUpdate.KeyChar <= '4')
                                {
                                    update.AskConfirmation();
                                    string confirmation_4 = Console.ReadLine().ToLower();

                                    if (confirmation_4 == "y" || confirmation_4 == "yes")
                                    {
                                        viewModel.input = updateName;
                                        viewModel.UpdateWork();
                                        view.ShowWorkUpdatedMessage();
                                    }
                                }
                            }
                            else
                            {
                                view.ShowNoWorkAvailable();
                            }
                        }

                        Console.Clear();
                        break;

                    case '5': // Launch a work
                        view.ShowList(viewModel);
                        if (viewModel.GetWorkCount() > 0)
                        {
                            string workName = view.AskWorkName();
                            viewModel.input = workName;
                            var workToLaunch = viewModel.GetWorkByName();
                            if (workToLaunch != null)
                            {
                                view.ShowWork(workToLaunch);
                                view.AskConfirmation();
                                string confirmation_5 = Console.ReadLine().ToLower();
                                if (confirmation_5 == "y" || confirmation_5 == "yes")
                                {

                                    DailyLog logger = DailyLog.getInstance();
                                    logger.createLogFile();


                                    viewModel._currentWork = workToLaunch;


                                    logger.TransferFilesWithLogs(
                                        workToLaunch.GetSource(),  // Source path
                                        workToLaunch.GetTarget(),  // Destination path
                                        workToLaunch.GetName()     // Work name
                                    );

                                    view.WarnLaunch();

                                }
                            }
                            else
                            {
                                view.ShowNoWorkAvailable();
                            }
                        }
                        Console.Clear();
                        break;

                    case '6': // Change language
                        language.language = view.language;
                        language.ShowAvailableLanguage();

                        view.ShowMessageChoice();

                        ConsoleKeyInfo languageChoice = Console.ReadKey();
                        switch (languageChoice.KeyChar)
                        {
                            case '1':
                                if (view.language == "English")
                                {
                                    language.WarnCurrrentLanguage();
                                    view.ShowNext();
                                }
                                else
                                {
                                    view.AskConfirmation();
                                    string confirmation_6 = Console.ReadLine();
                                    if (confirmation_6 == "y" || confirmation_6 == "yes")
                                    {
                                        view.language = "English";
                                        view.ShowNext();
                                    }
                                    else if (confirmation_6 == "n" || confirmation_6 == "no")
                                    {
                                        language.ShowAvailableLanguage();
                                    }
                                    else
                                    {
                                        view.WarnInvalidOption();
                                    }
                                }
                                break;
                            case '2':
                                if (view.language == "French")
                                {
                                    language.WarnCurrrentLanguage();
                                    view.ShowNext();
                                }
                                else
                                {
                                    view.AskConfirmation();
                                    string confirmation_6 = Console.ReadLine();
                                    if (confirmation_6 == "y" || confirmation_6 == "yes")
                                    {
                                        view.language = "French";
                                        view.ShowNext();
                                    }
                                    else if (confirmation_6 == "n" || confirmation_6 == "no")
                                    {
                                        language.ShowAvailableLanguage();
                                    }
                                    else
                                    {
                                        view.WarnInvalidOption();
                                    }
                                }
                                break;
                        }

                        Console.Clear();
                        break;

                    case '7':// Exit the program
                        view.AskConfirmation();
                        string confirmation_7 = Console.ReadLine().ToLower();

                        if (confirmation_7 == "y" || confirmation_7 == "yes")
                        {
                            exit = true;
                        }

                        Console.Clear();
                        break;

                    case '8': // Export logs to JSON/XML
                        // Utilisation de la même instance de DailyLog sans redéclarer la variable
                        DailyLog logManager = DailyLog.getInstance();

                        // Si les logs n'ont pas encore été initialisés, on crée un fichier journal
                        if (logManager.GetLogEntries().Count == 0)
                        {
                            logManager.createLogFile();
                        }

                        // Afficher les statistiques des logs actuels
                        var stats = logManager.GetLogStatistics();
                        Console.WriteLine("Current logs statistics:");
                        Console.WriteLine($"- Total entries: {stats["totalEntries"]}");
                        if (Convert.ToInt32(stats["totalEntries"]) > 0)
                        {
                            Console.WriteLine($"- Total file size: {stats["totalFileSize"]} bytes");
                            Console.WriteLine($"- Average file size: {stats["averageFileSize"]} KB");
                            Console.WriteLine($"- Total transfer time: {stats["totalTransferTime"]} seconds");
                            Console.WriteLine($"- Average transfer time: {stats["averageTransferTime"]} seconds");
                            Console.WriteLine($"- First transfer: {stats["startTime"]}");
                            Console.WriteLine($"- Last transfer: {stats["endTime"]}");
                        }
                        Console.WriteLine();

                        // Menu pour choisir le format d'export
                        Console.WriteLine("Choose export format:");
                        Console.WriteLine("1. JSON");
                        Console.WriteLine("2. XML");
                        Console.WriteLine("0. Cancel");

                        ConsoleKeyInfo exportChoice = Console.ReadKey();
                        Console.WriteLine();

                        if (exportChoice.KeyChar == '0')
                        {
                            Console.WriteLine("Export cancelled.");
                        }
                        else if (exportChoice.KeyChar == '1' || exportChoice.KeyChar == '2')
                        {
                            // Demander le chemin du fichier
                            Console.WriteLine("Enter export file path:");
                            if (exportChoice.KeyChar == '1')
                                Console.WriteLine("Example: C:\\Logs\\export.json");
                            else
                                Console.WriteLine("Example: C:\\Logs\\export.xml");

                            string exportPath = Console.ReadLine();

                            // Vérifier que le chemin est valide
                            if (string.IsNullOrWhiteSpace(exportPath))
                            {
                                Console.WriteLine("Invalid path. Export cancelled.");
                            }
                            else
                            {
                                try
                                {
                                    // Exporter selon le format choisi
                                    if (exportChoice.KeyChar == '1')
                                    {
                                        logManager.ExportToJson(exportPath);
                                        Console.WriteLine($"Logs successfully exported to JSON: {exportPath}");
                                    }
                                    else
                                    {
                                        logManager.ExportToXml(exportPath);
                                        Console.WriteLine($"Logs successfully exported to XML: {exportPath}");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error during export: {ex.Message}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice.");
                        }

                        Console.WriteLine("Press any key to return to menu...");
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    default:
                        view.WarnInvalidOption();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }

            view.ShowLeaveMessage();
            Console.ReadKey();
            Console.Clear();
        }
    }
}