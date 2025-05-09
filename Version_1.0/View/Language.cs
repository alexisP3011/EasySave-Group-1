using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version_1._0.View
{
    class Language : View
    {
        public Language() { }

        public void WarnCurrrentLanguage()
        {
            if (language == "English")
            {
                Console.WriteLine("The selected language is already in use.");
            }
            else if (language == "French")
            {
                Console.WriteLine("La langue sélectionnée est déjà en cours d'usage.");
            }
        }
        public void ShowAvailableLanguage()
        {
            if (language == "English")
            {
                Console.WriteLine("Our application is available in these two languages:");
                Console.WriteLine("1. English");
                Console.WriteLine("2. French");
            }
            else if (language == "French")
            {
                Console.WriteLine("Notre application est disponible dans ces deux langues :");
                Console.WriteLine("1. Anglais");
                Console.WriteLine("2. Français");
            }
        }

        public void WarnChangedLanguage()
        {
            if (language == "English")
            {
                Console.WriteLine("The language has been changed successfully.");
            }
            else if (language == "French")
            {
                Console.WriteLine("La langue a été changée avec succès.");
            }
        }
    }   
}