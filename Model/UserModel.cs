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
        public string Password { get; set; }
        public string Hint { get; set; }
        public string Email { get; set; }

        public UserModel() { }

        public UserModel(string id, string email, 
                         string hint, string encryptedPassword)
        {
            ID = id;
            Email = email;
            Password = encryptedPassword;
            Hint = hint;
        }

        public override string ToString()
        {
            return $"ID: {ID}, HashedPassword: {Password}, Hint: {Hint}, Email: {Email}";
        }
    }
}
