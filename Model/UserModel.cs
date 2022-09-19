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
        public string ID { get; set; }
        public string Username { get; set; }    
        public string Email { get; set; }
        public string Password { get; set; }
        public string Hint { get; set; }
        public string Nonce { get; set; }

        public UserModel(string iD, string username, string email, string password, 
                         string hint, string nonce)
        {
            ID = iD;
            Username = username;
            Email = email;
            Password = password;
            Hint = hint;
            Nonce = nonce;
        }
    }
}
