using Newtonsoft.Json;
using SnailPass_Desktop.Model.Cryptography;
using System;

namespace SnailPass_Desktop.Model
{
    public class AccountModel : ICloneable
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [CryptableField]
        [JsonProperty("name")]
        public string ServiceName { get; set; }

        [CryptableField]
        [JsonProperty("login")]
        public string Login { get; set; }

        [CryptableField]
        [JsonProperty("encrypted_password")]
        public string Password { get; set; }

        [JsonProperty("is_favorite")]
        public bool IsFavorite { get; set; }

        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("creation_time")]
        public string CreationTime { get; set; }

        [JsonProperty("update_time")]
        public string UpdateTime { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        public AccountModel() { }

        public AccountModel(string iD, string serviceName, string login, string password, 
            bool isFavorite, bool isDeleted, string creationTime, string updateTime, string userId)
        {
            ID = iD;
            ServiceName = serviceName;
            Login = login;
            Password = password;
            IsFavorite = isFavorite;
            IsDeleted = isDeleted;
            CreationTime = creationTime;
            UpdateTime = updateTime;
            UserId = userId;
        }

        public override string ToString()
        {
            return $"ID: {ID}, Serivce name: {ServiceName}, Login: {Login}, Password(enc): {Password}, " +
                $"Is favorite: {IsFavorite}, Is deleted: {IsDeleted}, " +
                $"Creation time: {CreationTime}, Update time: {UpdateTime}, User ID: {UserId}";
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
