using System;
using System.IO;
using System.Threading;

namespace CryptoSoft
{
    public static class Program
    {
        private static Mutex? _mutex;
        private const string MutexName = "CryptoSoftAppMutex";

        public static void Main(string[] args)
        {
            bool createdNew;
            _mutex = new Mutex(true, MutexName, out createdNew);

            if (!createdNew)
            {
                Console.WriteLine("Another instance of the application is already running.");
                Environment.Exit(-2);
                return;
            }

            try
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Usage: CryptoSoft <inputFile> <outputFile>");
                    Environment.Exit(-1);
                    return;
                }

                string filePath = args[0];
                string key = args[1];

                var fileManager = new FileManager(filePath, key);
                int elapsedMs = fileManager.TransformFile();

                Console.WriteLine($"File processed in {elapsedMs} ms.");

                Thread.Sleep(1000); // Simulate some processing time

                Environment.Exit(0);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {ex.Message}");
                Environment.Exit(-1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                Environment.Exit(-1);
            }
            finally
            {
                _mutex.ReleaseMutex();
                _mutex.Dispose();
            }
        }
    }
}