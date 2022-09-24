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
        public string ID { get; set; }
        public string Username { get; set; }    
        public string Email { get; set; }
        public string Hint { get; set; }
        public string Nonce { get; set; }
        public string EncryptedPassword { get; set; }

        public UserModel() { }

        public UserModel(string id, string username, string email, 
                         string hint, string nonce, string encryptedPassword)
        {
            ID = id;
            Username = username;
            Email = email;
            Hint = hint;
            Nonce = nonce;
            EncryptedPassword = encryptedPassword;
        }
    }
}
