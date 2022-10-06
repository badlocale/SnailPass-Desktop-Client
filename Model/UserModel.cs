using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model
{
    public class UserModel
    {
        private string name;

        public string ID { get { return name; } set { name = value; } }
        public string Login { get; set; }    
        public string Password { get; set; }
        public string Hint { get; set; }
        public string Email { get; set; }
        public string Nonce { get; set; }

        public UserModel() { }

        public UserModel(string id, string username, string email, 
                         string hint, string nonce, string encryptedPassword)
        {
            ID = id;
            Login = username;
            Email = email;
            Hint = hint;
            Nonce = nonce;
            Password = encryptedPassword;
        }
    }
}
