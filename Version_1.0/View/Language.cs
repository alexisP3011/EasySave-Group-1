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
            Console.WriteLine(_rm.GetString("AlreadyInUse"));
        }
        public void ShowAvailableLanguage()
        {
            Console.WriteLine(_rm.GetString("LanguageChoices"));

        }

        public void WarnChangedLanguage()
        {
            Console.WriteLine(_rm.GetString("ChangeLanguageMessage."));
        }
    }   
}