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
        public (string, string) Encrypt(SecureString password, SecureString master, byte[]? nonce = null)
        {
            byte[] byteEncData;
            byte[] byteNonce;
            (byteEncData, byteNonce) = EncryptInternal(Encoding.UTF8.GetBytes(SecureStringToString(password)), 
                                                       Encoding.UTF8.GetBytes(SecureStringToString(master)));
            return (Convert.ToBase64String(byteEncData), Encoding.UTF8.GetString(byteNonce));
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
                aes.IV = nonce;

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

        public string Decrypt(string encryptedData, string key, byte[] nonce)
        {
            byte[] encryptedBytes = Encoding.UTF8.GetBytes(encryptedData);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            return DecryptInternal(encryptedBytes, keyBytes, nonce);
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
