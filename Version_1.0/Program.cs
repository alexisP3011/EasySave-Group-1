using System;
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

                            add.AskSaveName();
                            viewModel.SetName(Console.ReadLine());

                            add.AskSource();
                            viewModel.SetSource(Console.ReadLine());

                            add.AskTarget();
                            viewModel.SetTarget(Console.ReadLine());

                            add.AskType();
                            viewModel.SetType(Console.ReadLine());

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
                            Console.WriteLine("You have reached the maximum number of backup works (5). Please delete one before creating a new one.");
                            Console.ReadKey();
                            Console.Clear();
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
                                        viewModel.SetName(Console.ReadLine());
                                        break;

                                    case '2':
                                        update.AskSource();
                                        viewModel.SetSource(Console.ReadLine());
                                        break;
                                    case '3':
                                        update.AskTarget();
                                        viewModel.SetTarget(Console.ReadLine());
                                        break;
                                    case '4':
                                        update.AskType();
                                        viewModel.SetType(Console.ReadLine());
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
                                
                                if (confirmation_5 == "y" || confirmation_5 == "yes")
                                {

                                    viewModel.LaunchWork(workToLaunch);
                                     
                                    Console.WriteLine("Launching the backup work...");
                                    Console.WriteLine("Press any key to continue...");
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
                        var language = new View.Language();
                        language.ShowCurrentLanguage();
                        language.ShowAvailableLanguage();
                        language.ShowMessageChoice();
                        string languageChoice = Console.ReadLine();
                        language.AskConfirmation();
                        string confirmation_6 = Console.ReadLine();
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
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.WriteLine("Press any key to continue...");
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