using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SnailPass_Desktop.Model.Cryptography
{
    public class CryptographerBase
    {
        protected string SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}
