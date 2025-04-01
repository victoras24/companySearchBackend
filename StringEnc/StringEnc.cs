using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CompanySearchBackend.StringEnc
{
    public class StringDecryptor
    {
        private readonly string _encryptionKey;

        public StringDecryptor(string encryptionKey)
        {
            if (string.IsNullOrEmpty(encryptionKey) || encryptionKey.Length < 16)
            {
                throw new ArgumentException("The encryption key should be at least 16 characters long.");
            }
            _encryptionKey = encryptionKey;
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                throw new ArgumentException("Cipher text cannot be null or empty");
            }

            try
            {
                var fullCipher = Convert.FromBase64String(cipherText);

                // Check if the cipher text is long enough to contain the IV
                if (fullCipher.Length < 16)
                {
                    throw new ArgumentException("Cipher text is too short to contain an IV");
                }

                using var aesAlg = Aes.Create();
                var iv = new byte[16];
                Array.Copy(fullCipher, 0, iv, 0, iv.Length);

                var cipherBytes = new byte[fullCipher.Length - iv.Length];
                Array.Copy(fullCipher, iv.Length, cipherBytes, 0, cipherBytes.Length);

                aesAlg.Key = Encoding.UTF8.GetBytes(_encryptionKey.PadRight(32).Substring(0, 32));
                aesAlg.IV = iv;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using var msDecrypt = new MemoryStream(cipherBytes);
                using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt);

                return srDecrypt.ReadToEnd();
            }
            catch (FormatException)
            {
                throw new ArgumentException("The input is not a valid Base-64 string");
            }
            catch (CryptographicException ex)
            {
                throw new ArgumentException($"Decryption failed: {ex.Message}");
            }
        }
    }
}