using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Version_3._0.Model
{
    /// <summary>
    /// File manager class to encrypt files with XOR
    /// </summary>
    public class FileManager
    {
        private string FilePath { get; }
        private string Key { get; }

        private const string Signature = "CryptoSoft";

        public FileManager(string path, string key)
        {
            FilePath = path;
            Key = key;
        }

        public bool IsEncrypted()
        {
            if (!File.Exists(FilePath)) return false;

            using FileStream fs = new(FilePath, FileMode.Open, FileAccess.Read);
            byte[] header = new byte[Signature.Length];
            fs.Read(header, 0, header.Length);
            string headerText = Encoding.UTF8.GetString(header);
            return headerText == Signature;
        }

        public int TransformFile()
        {
            if (!File.Exists(FilePath))
                throw new FileNotFoundException("Fichier introuvable");

            if (IsEncrypted())
                return -1;

            Stopwatch stopwatch = Stopwatch.StartNew();

            byte[] fileBytes = File.ReadAllBytes(FilePath);
            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
            byte[] encryptedBytes = Xor(fileBytes, keyBytes);

            byte[] header = Encoding.UTF8.GetBytes(Signature);
            byte[] result = new byte[header.Length + encryptedBytes.Length];
            Buffer.BlockCopy(header, 0, result, 0, header.Length);
            Buffer.BlockCopy(encryptedBytes, 0, result, header.Length, encryptedBytes.Length);

            File.WriteAllBytes(FilePath, result);
            stopwatch.Stop();
            return (int)stopwatch.ElapsedMilliseconds;
        }

        private static byte[] Xor(IReadOnlyList<byte> fileBytes, IReadOnlyList<byte> keyBytes)
        {
            byte[] result = new byte[fileBytes.Count];
            for (int i = 0; i < fileBytes.Count; i++)
                result[i] = (byte)(fileBytes[i] ^ keyBytes[i % keyBytes.Count]);
            return result;
        }
    }
}
