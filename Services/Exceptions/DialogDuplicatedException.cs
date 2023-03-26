using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass.Services.Exceptions
{
    public class DialogDuplicatedException : Exception
    {
        public DialogDuplicatedException(string message) : base(message)
        {

        }
    }
}
