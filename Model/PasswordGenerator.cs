using SnailPass.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SnailPass.Model
{
    public class PasswordGenerator : IPasswordGenerator
    {
        private const string LowercaseAlphabet = "abcdefghijklmnopqrstuvwxyz";
        private const string UppercaseAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string DigitAlphabet = "0123456789";
        private const string SpecialAlphabet = "~!@#$%^&*()_-+={}[]\\|:;\"'<>,.?/";

        public string Generate(int length, bool isLowercase, bool isUppercase, bool isDigits, bool isSpecials)
        {
            if ((!isLowercase && !isUppercase && !isDigits && !isSpecials) || length == 0)
            {
                return string.Empty;
            }

            StringBuilder composedAlphabet = new();
            char[] password = new char[length];
            List<int> filledPositions = new();

            if (isLowercase)
            {
                SetRandomChar(filledPositions, password, LowercaseAlphabet);
                composedAlphabet.Append(LowercaseAlphabet);
            }

            if (isUppercase)
            {
                SetRandomChar(filledPositions, password, UppercaseAlphabet);
                composedAlphabet.Append(UppercaseAlphabet);
            }

            if (isDigits)
            {
                SetRandomChar(filledPositions, password, DigitAlphabet);
                composedAlphabet.Append(DigitAlphabet);
            }

            if (isSpecials)
            {
                SetRandomChar(filledPositions, password, SpecialAlphabet);
                composedAlphabet.Append(SpecialAlphabet);
            }

            while (filledPositions.Count < length)
            {
                SetRandomChar(filledPositions, password, composedAlphabet.ToString());
            }

            return new string(password);
        }

        private void SetRandomChar(List<int> filledPositions, char[] password, string alphabet)
        {
            int passwordLength = password.Length;
            int alphabetLength = alphabet.Length;
            char symbol = alphabet[RandomNumberGenerator.GetInt32(alphabetLength - 1)];
            int position = RandomNumberGenerator.GetInt32(passwordLength - 1);

            while (filledPositions.Contains(position))
            {
                position++;
                if (position >= passwordLength)
                {
                    position = 0;
                }
            }

            filledPositions.Add(position);
            password[position] = symbol;
        }
    }
}
