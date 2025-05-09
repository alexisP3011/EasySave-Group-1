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
                            viewModel.input = Console.ReadLine() ;
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
                            var workToDelete = viewModel.GetWorkByName(workName);

                            if (workToDelete != null)
                            {
                                view.ShowWork(workToDelete);
                                view.AskConfirmation();
                                string confirmation_3 = Console.ReadLine().ToLower();

                                if (confirmation_3 == "y" || confirmation_3 == "yes")
                                {
                                    viewModel.DeleteWork(workName);
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

                            var workToUpdate = viewModel.GetWorkByName(updateName);
                            if (workToUpdate != null)
                            {
                                viewModel.LoadWorkToCurrent(updateName);
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
                                        viewModel.UpdateWork(updateName);
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
                            var workToLaunch = viewModel.GetWorkByName(workName);

                            if (workToLaunch != null)
                            {
                                view.ShowWork(workToLaunch);
                                view.AskConfirmation();
                                string confirmation_5 = Console.ReadLine().ToLower();

                                Stopwatch chrono = new Stopwatch();

                                if (confirmation_5 == "y" || confirmation_5 == "yes")
                                {
                                    chrono.Start();
                                    viewModel.LaunchWork(workToLaunch);
                                    chrono.Stop();

                                    view.WarnLaunch();

                                    DailyLog logger = DailyLog.getInstance();

                                    logger.createLogFile();
                                    logger.AddLogEntry(workToLaunch.GetName(), workToLaunch.GetSource(), workToLaunch.GetTarget(), logger.CountFilesInSource(workToLaunch.GetSource()), chrono.ElapsedMilliseconds);
                                    logger.SaveLogs();

                                    Console.ReadKey();
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