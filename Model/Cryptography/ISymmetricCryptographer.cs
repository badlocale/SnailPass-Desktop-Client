using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desctop.Model.Cryptography
{
    internal interface ISymmetricCryptographer
    {
        public (byte[], byte[]) Encrypt(string data, byte[] key, byte[]? nonce);

        public string Decrypt(byte[] encryptedData, byte[] IV, byte[] key);


    }
}
