using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desctop.Model
{
    public class UserModel
    {
        public Guid ID { get; set; }
        public string Email { get; set; }
        public string EncKey { get; set; }
        public string PrivKey { get; set; }
    }
}
