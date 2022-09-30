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
        public string id { get; set; }
        public string login { get; set; }    
        public string master_password_hash { get; set; }
        public string hint { get; set; }
        public string Email { get; set; }
        public string Nonce { get; set; }

        public UserModel() { }

        public UserModel(string id, string username, string email, 
                         string hint, string nonce, string encryptedPassword)
        {
            this.id = id;
            login = username;
            Email = email;
            this.hint = hint;
            Nonce = nonce;
            master_password_hash = encryptedPassword;
        }
    }
}
