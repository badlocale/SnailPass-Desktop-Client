using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desctop.Model.Cryptography
{
    static class AesCbc
    {
        public static (byte[], byte[]) Encrypt(string data, byte[] key)
        {
            byte[] encryptedData;
            byte[] IV = new byte[16];

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = key.Length * 8;
                aes.Key = key;
                aes.GenerateIV();
                aes.Mode = CipherMode.CBC;

                IV = aes.IV;

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(data);
                        }

                        encryptedData = ms.ToArray();
                    }
                }

                return (encryptedData, IV);
            }
        }

        public static string Decrypt(byte[] encryptedData, byte[] IV, byte[] key)
        {
            string decryptedText = null;

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = key.Length * 8;
                aes.Key = key;
                aes.IV = IV;
                aes.Mode = CipherMode.CBC;

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream(encryptedData))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sw = new StreamReader(cs))
                        {
                            decryptedText = sw.ReadToEnd();
                        }
                    }
                }
            }

            return decryptedText;
        }
    }
}
