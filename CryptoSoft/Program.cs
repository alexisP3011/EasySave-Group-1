namespace CryptoSoft;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== CryptoSoft ===\n");

        bool continuer = true;

        while (continuer)
        {
            try
            {
                Console.Write("Enter the path of the file to encrypt/decrypt: ");
                string filePath = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Enter your secret key: ");
                string key = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(key))
                {
                    Console.WriteLine("Error: the file path and the key must be provided.\n");
                    continue;
                }

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Error: File not found at '{Path.GetFullPath(filePath)}'\n");
                    continue;
                }

                var fileManager = new FileManager(filePath, key);
                int elapsedTime = fileManager.TransformFile();

                Console.WriteLine($"File transformed in {elapsedTime} ms.\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message + "\n");
            }

            Console.Write("Do you want to encrypt/decrypt another file? (y/n): ");
            string? answer = Console.ReadLine()?.Trim();

            continuer = answer == "y";
            Console.WriteLine();
        }

        Console.WriteLine("Thank you for using CryptoSoft!");
    }
}