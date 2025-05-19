namespace CryptoSoft;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== CryptoSoft ===");

        string[] allowedExtensions = { ".txt", ".csv", ".json", ".xml", ".log" };
        bool keep = true;

        while (keep)
        {
            try
            {
                Console.Write("Enter the path of the file to encrypt: ");
                string filePath = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Enter your secret key: ");
                string key = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(key))
                {
                    Console.WriteLine("Error: the file path and the key must be provided.");
                    continue;
                }

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Error: File not found at {Path.GetFullPath(filePath)}");
                    continue;
                }

                string extension = Path.GetExtension(filePath).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    Console.WriteLine($"The file type {extension} is not supported. Allowed types are: {string.Join(", ", allowedExtensions)}.");
                    continue;
                }

                var fileManager = new FileManager(filePath, key);
                int elapsedTime = fileManager.TransformFile();

                Console.WriteLine($"File '{Path.GetFileName(filePath)}' has been encrypted successfully in {elapsedTime} ms.");  
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            Console.Write("Do you want to encrypt/decrypt another file? (y/n): ");
            string? answer = Console.ReadLine()?.Trim();

            keep = answer == "y";
            Console.WriteLine();
        }

        Console.WriteLine("Thank you for using CryptoSoft!");
    }
}