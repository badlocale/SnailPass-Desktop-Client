﻿using Newtonsoft.Json;
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
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("master_password_hash")]
        public string Password { get; set; }

        [JsonProperty("hint")]
        public string Hint { get; set; }

        [JsonProperty("email")]
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
