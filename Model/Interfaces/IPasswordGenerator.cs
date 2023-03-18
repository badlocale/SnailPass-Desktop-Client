using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass.Model.Interfaces
{
    public interface IPasswordGenerator
    {
        public string Generate(int lenght, bool isLowercase, bool isUppercase, bool isDigits, bool isSpecials);
    }
}
