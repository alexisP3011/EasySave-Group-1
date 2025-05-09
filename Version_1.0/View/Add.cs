using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version_1._0.View
{
    class Add : View
    {
        public Add() { }

        public void AskSaveName()
        {
            Console.WriteLine();
            if (language == "English")
            {
                Console.WriteLine("Please enter the name of the backup work: ");
            }
            else if (language == "French")
            {
                Console.WriteLine("Veuillez entrer le nom du travail de sauvegarde : ");
            }
        }

        public void AskSource()
        {
            Console.WriteLine();
            if (language == "English")
            {
                Console.WriteLine("Please enter your source: ");
            }
            else if (language == "French")
            {
                Console.WriteLine("Veuillez entrer votre source : ");
            }
        }
        public void AskTarget()
        {
            Console.WriteLine();
            if (language == "English")
            {
                Console.WriteLine("Please enter your target: ");
            }
            else if (language == "French")
            {
                Console.WriteLine("Veuillez entrer votre cible : ");
            }
        }

        public void AskType()
        {
            Console.WriteLine();
            if (language == "English")
            {
                Console.WriteLine("Please enter the type of the backup work: ");
            }
            else if (language == "French")
            {
                Console.WriteLine("Veuillez entrer le type du travail de sauvegarde : ");
            }
        }


    }
}
