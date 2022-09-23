using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Cryptography
{
    public class AesCbc
    {
        public (byte[], byte[]) Encrypt(string data, byte[] key, byte[]? nonce)
        {
            byte[] encryptedData;
            byte[] IV = new byte[16];

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = key.Length * 8;
                aes.Key = key;
                aes.Mode = CipherMode.CBC;

                if (nonce == null || nonce.Length != 16)
                {
                    aes.GenerateIV();
                }
                else
                {
                    aes.IV = nonce;
                }

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

        public string Decrypt(byte[] encryptedData, byte[] IV, byte[] key)
        {
            string decryptedText = null;

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = key.Length * 8;
                aes.Key = key;
                aes.Mode = CipherMode.CBC;
                aes.IV = IV;

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
