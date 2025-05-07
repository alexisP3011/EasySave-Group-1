using System;
using Version_1._0.View;


namespace Version_1._0
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            var view = new View.View(); 

            while (!exit)
            {
                view.ShowMenu();
                
                ConsoleKeyInfo choice = Console.ReadKey();
                Console.Clear();
                switch (choice.KeyChar)
                {

                    case '1':
                        
                        var add = new View.Add();
                        add.AskSaveName();
                        string? saveName = Console.ReadLine();

                        add.AskSource();
                        string? source = Console.ReadLine();

                        add.AskTarget();
                        string? target = Console.ReadLine();

                        add.AskType();
                        string? type = Console.ReadLine();

                        view.AskConfirmation();
                        string? confirmation = Console.ReadLine();

                        Console.Clear();
                        break;

                    case '2':
                        view.ShowList();

                       
                        Console.ReadKey(); 
                        Console.Clear();
                  
                        break;
                    case '3':
                        var delete = new View.Add();
                        view.ShowList();
                        delete.AskSaveName();
                        string? deleteName = Console.ReadLine();
                        view.AskConfirmation();
                        string? confirmation_3 = Console.ReadLine();
                        Console.Clear();
                        break;

                    case '4':

                        var update = new View.Update();
                        update.ShowList();
                        update.AskSaveName();
                        string? updateName = Console.ReadLine();
                        update.ShowWork();
                        update.AskItemToUpdate();

                        ConsoleKeyInfo itemToUpdate = Console.ReadKey(); 

                        switch (itemToUpdate.KeyChar) 
                        {
                            case '1':
                                update.AskSaveName();
                                string? newName = Console.ReadLine();
                                break;

                            case '2':
                                update.AskSource();
                                string? newSource = Console.ReadLine();
                                break;
                            case '3':
                                update.AskTarget();
                                string? newTarget = Console.ReadLine();
                                break;
                            case '4':
                                update.AskType();
                                string? newType = Console.ReadLine();
                                break;
                            case '5':
                                break;
                            default:
                                view.WarnInputError();
                                break;
                        }
     
                        

                        update.AskConfirmation(); 
                        string? confirmation_4 = Console.ReadLine();
                        Console.Clear();
                      
           
                        break;
                    case '5':
                        view.ShowList();
                        view.ShowMessageChoice();
                        string? workToLaunch = Console.ReadLine();
                        view.ShowWork();
                        view.AskConfirmation();
                        string? confirmation_5 = Console.ReadLine();
                        Console.Clear();


                        break;
                    case '6':
                        var language = new View.Language();
                        language.ShowCurrentLanguage();
                        language.ShowAvailableLanguage();
                        language.ShowMessageChoice();
                        string? languageChoice = Console.ReadLine();
                        language.AskConfirmation();
                        string? confirmation_6 = Console.ReadLine();
                        Console.Clear();

                        break;
                    case '7':
                        view.AskConfirmation();
                        string? confirmation_7 = Console.ReadLine();

                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }

            view.ShowLeaveMessage();
            Console.ReadKey();
            Console.Clear();
        }
    }
}