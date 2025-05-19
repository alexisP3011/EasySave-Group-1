using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Version_2._0.Model;

namespace Version_2._0.Model
{
    /// <summary>
    /// Service de cryptage utilisant CryptoSoft
    /// </summary>
    public class CryptoService
    {
        // Singleton pour le service de cryptage
        private static CryptoService _instance;
        public static CryptoService Instance
        {
            get => _instance ??= new CryptoService();
        }

        // Signature pour vérifier si un fichier est déjà crypté
        private const string Signature = "CryptoSoft";

        /// <summary>
        /// Vérifie si le fichier peut être crypté selon les extensions autorisées
        /// </summary>
        public bool CanEncrypt(string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            string extension = Path.GetExtension(filePath).ToLower();
            return Settings.Instance.AllowedExtensions.Contains(extension);
        }

        /// <summary>
        /// Vérifie si un fichier est déjà crypté
        /// </summary>
        public bool IsEncrypted(string filePath)
        {
            try
            {
                using FileStream fs = new(filePath, FileMode.Open, FileAccess.Read);
                byte[] header = new byte[Signature.Length];
                fs.Read(header, 0, header.Length);
                string headerText = Encoding.UTF8.GetString(header);
                return headerText == Signature;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Crypte un fichier et retourne le temps d'exécution en millisecondes
        /// </summary>
        public int EncryptFile(string filePath)
        {
            if (!CanEncrypt(filePath))
                throw new ArgumentException($"Le fichier '{filePath}' ne peut pas être crypté.");

            if (IsEncrypted(filePath))
                throw new InvalidOperationException("Le fichier est déjà crypté.");

            // Utiliser la clé de cryptage depuis les paramètres
            string key = Settings.Instance.EncryptionKey;

            Stopwatch stopwatch = Stopwatch.StartNew();
            byte[] fileBytes = File.ReadAllBytes(filePath);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] encryptedBytes = XorEncrypt(fileBytes, keyBytes);

            // Ajouter l'en-tête de signature
            byte[] header = Encoding.UTF8.GetBytes(Signature);
            byte[] result = new byte[header.Length + encryptedBytes.Length];
            Buffer.BlockCopy(header, 0, result, 0, header.Length);
            Buffer.BlockCopy(encryptedBytes, 0, result, header.Length, encryptedBytes.Length);

            File.WriteAllBytes(filePath, result);
            stopwatch.Stop();

            return (int)stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Algorithme de cryptage XOR
        /// </summary>
        private byte[] XorEncrypt(byte[] data, byte[] key)
        {
            var result = new byte[data.Length];
            for (var i = 0; i < data.Length; i++)
            {
                result[i] = (byte)(data[i] ^ key[i % key.Length]);
            }
            return result;
        }
    }
}
