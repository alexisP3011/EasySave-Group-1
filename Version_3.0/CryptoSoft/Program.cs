using System;
using System.IO;
using System.Threading;

namespace CryptoSoft
{
    public static class Program
    {
        private static Mutex? _mutex;
        private const string MutexName = "CryptoSoft_Mutex";

        public static void Main(string[] args)
        {
            bool createdNew;
            _mutex = new Mutex(true, MutexName, out createdNew);

            if (!createdNew)
            {
                Console.WriteLine("Instance déjà en cours.");
                Environment.Exit(-2);
                return;
            }

            try
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Arguments manquants : <chemin_fichier> <clé>");
                    Environment.Exit(-1);
                    return;
                }

                string filePath = args[0];
                string key = args[1];

                var fileManager = new FileManager(filePath, key);

                int elapsedMs = fileManager.TransformFile();
                Console.WriteLine($"Fichier traité en {elapsedMs} ms");

                Thread.Sleep(10000); // Pause pour permettre à l'utilisateur de lire le message

                Environment.Exit(0);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Fichier introuvable : " + ex.Message);
                Environment.Exit(-3);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                Environment.Exit(-99);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }
    }
}