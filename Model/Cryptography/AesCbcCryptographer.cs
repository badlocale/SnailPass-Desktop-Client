using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Cryptography
{
    public class AesCbcCryptographer : CryptographerBase, ISymmetricCryptographer
    {
        public (string, string) Encrypt(SecureString password, string key, byte[]? nonce = null)
        {
            byte[] byteEncData;
            byte[] byteNonce;
            (byteEncData, byteNonce) = EncryptInternal(Encoding.UTF8.GetBytes(SecureStringToString(password)), 
                                                       Convert.FromBase64String(key));
         
            return (Convert.ToBase64String(byteEncData), Convert.ToBase64String(byteNonce));
        }

        private (byte[], byte[]) EncryptInternal(byte[] data, byte[] key, byte[]? nonce = null)
        {
            byte[] encryptedData;
            byte[] IV = new byte[16];

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = key.Length * 8;
                aes.Key = key;
                aes.Mode = CipherMode.CBC;

                if (nonce == null)
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
                            sw.Write(Encoding.UTF8.GetString(data));
                        }

                        encryptedData = ms.ToArray();
                    }
                }

                return (encryptedData, IV);
            }
        }

        public string Decrypt(string encryptedData, string key, string nonce)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
            byte[] keyBytes = Convert.FromBase64String(key);
            byte[] nonceBytes = Convert.FromBase64String(nonce);
            return DecryptInternal(encryptedBytes, keyBytes, nonceBytes);
        }

        private string DecryptInternal(byte[] encryptedData, byte[] key, byte[] nonce)
        {
            string decryptedText = null;

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = key.Length * 8;
                aes.Key = key;
                aes.Mode = CipherMode.CBC;
                aes.IV = nonce;

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
