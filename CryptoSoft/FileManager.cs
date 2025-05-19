using System.Diagnostics;
using System.Text;

namespace CryptoSoft;

/// <summary>
/// File manager class
/// This class is used to encrypt files
/// </summary>
public class FileManager(string path, string key)
{
    private string FilePath { get; } = path;
    private string Key { get; } = key;

    private const string Signature = "CryptoSoft";

    /// <summary>
    /// Detects if the file has already been encrypted by checking for the header
    /// </summary>
    private bool IsEncrypted()
    {
        using FileStream fs = new(FilePath, FileMode.Open, FileAccess.Read); 
        byte[] header = new byte[Signature.Length];
        fs.Read(header, 0, header.Length);
        string headerText = Encoding.UTF8.GetString(header);
        return headerText == Signature;
    }

    /// <summary>
    /// Encrypts the file with xor encryption and add a signature to prevents re-encryption
    /// </summary>
    public int TransformFile()
    {
        if (IsEncrypted())
        {
            Console.WriteLine("The file is already encrypted.");
            return 0;
        }

        Stopwatch stopwatch = Stopwatch.StartNew();

        byte[] fileBytes = File.ReadAllBytes(FilePath);
        byte[] keyBytes = ConvertToByte(Key);
        byte[] encryptedBytes = XorMethod(fileBytes, keyBytes);

        byte[] header = Encoding.UTF8.GetBytes(Signature);
        byte[] result = new byte[header.Length + encryptedBytes.Length];
        Buffer.BlockCopy(header, 0, result, 0, header.Length);
        Buffer.BlockCopy(encryptedBytes, 0, result, header.Length, encryptedBytes.Length);

        File.WriteAllBytes(FilePath, result);

        stopwatch.Stop();
        return (int)stopwatch.ElapsedMilliseconds;
    }

    /// <summary>
    /// Convert a string in byte array
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private static byte[] ConvertToByte(string text)
    {
        return Encoding.UTF8.GetBytes(text);
    }

    /// <summary>
    /// </summary>
    /// <param name="fileBytes">Bytes of the file to convert</param>
    /// <param name="keyBytes">Key to use</param>
    /// <returns>Bytes of the encrypted file</returns>
    private static byte[] XorMethod(IReadOnlyList<byte> fileBytes, IReadOnlyList<byte> keyBytes)
    {
        var result = new byte[fileBytes.Count];
        for (var i = 0; i < fileBytes.Count; i++)
        {
            result[i] = (byte)(fileBytes[i] ^ keyBytes[i % keyBytes.Count]);
        }

        return result;
    }
}