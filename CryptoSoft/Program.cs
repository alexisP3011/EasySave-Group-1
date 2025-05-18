namespace CryptoSoft;

public static class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("=== CryptoSoft===");
            Console.Write("Enter the path of the file to encrypt: ");
            string filePath = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Enter your secret key: ");
            string key = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(key))
            {
                Console.WriteLine("Error: the file path and the key must be provided.");
                Environment.Exit(-1);
            }

            var fileManager = new FileManager(filePath, key);
            int ElapsedTime = fileManager.TransformFile();

            Console.WriteLine($"File transformed in {ElapsedTime} ms.");
            Environment.Exit(ElapsedTime);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
            Environment.Exit(-99);
        }
    }
}