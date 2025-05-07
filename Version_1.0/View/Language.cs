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
            Console.WriteLine("The selected language is already in use.");
        }
        public void ShowAvailableLanguage()
        {
            Console.WriteLine("Our application is available in these two languages:");
            Console.WriteLine("1. English");
            Console.WriteLine("2. French");

        }

        public void ShowCurrentLanguage()
        {
            Console.WriteLine("The current language is English.");
        }


        public void WarnChangedLanguage()
        {
            Console.WriteLine("The language has been changed successfully.");
        }
    }
    
    
}
