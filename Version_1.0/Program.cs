﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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

            while (!exit)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(viewModel.setLanguage());
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
                                    viewModel._currentWork = workToLaunch;

                                    // CHANGEMENT ICI
                                    string format = Version_1._0.Model.Settings.getInstance().LoadLogFormat().ToLower();
                                    // ← récupère le format depuis les paramètres
                                    viewModel.LaunchWorkWithFormat(format);

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
                        language.ShowAvailableLanguage();

                        view.ShowMessageChoice();

                        ConsoleKeyInfo languageChoice = Console.ReadKey();
                        switch (languageChoice.KeyChar)
                        {
                            case '1':
                                view.AskConfirmation();
                                string confirmation_6 = Console.ReadLine();
                                if (confirmation_6 == "y" || confirmation_6 == "yes")
                                {
                                    viewModel.input = "En";
                                    viewModel.changeLanguage();
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

                                break;

                            case '2':
                                view.AskConfirmation();
                                string confirmation_10 = Console.ReadLine();
                                if (confirmation_10 == "y" || confirmation_10 == "yes")
                                {
                                    viewModel.input = "Fr";
                                    viewModel.changeLanguage();
                                    view.ShowNext();
                                }
                                else if (confirmation_10 == "n" || confirmation_10 == "no")
                                {
                                    language.ShowAvailableLanguage();
                                }
                                else
                                {
                                    view.WarnInvalidOption();
                                }

                                break;
                        }

                        Console.Clear();
                        break;

                    case '7': // Choose log format (XML or JSON)
                        view.ShowLogFormatOptions();
                        ConsoleKeyInfo formatChoice = Console.ReadKey();
                        Console.WriteLine();

                        switch (formatChoice.KeyChar)
                        {
                            case '1': // XML format
                                view.AskConfirmation();
                                string confirmationXML = Console.ReadLine().ToLower();
                                if (confirmationXML == "y" || confirmationXML == "yes")
                                {
                                    viewModel.input = "XML";
                                    viewModel.SetLogFormat();
                                    view.ShowLogFormatUpdatedMessage("XML");
                                }
                                break;

                            case '2': // JSON format
                                view.AskConfirmation();
                                string confirmationJSON = Console.ReadLine().ToLower();
                                if (confirmationJSON == "y" || confirmationJSON == "yes")
                                {
                                    viewModel.input = "JSON";
                                    viewModel.SetLogFormat();
                                    view.ShowLogFormatUpdatedMessage("JSON");
                                }
                                break;

                            default:
                                view.WarnInvalidOption();
                                break;
                        }

                        Console.Clear();
                        break;

                    case '8':// Exit the program
                        view.AskConfirmation();
                        string confirmation_8 = Console.ReadLine().ToLower();

                        if (confirmation_8 == "y" || confirmation_8 == "yes")
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