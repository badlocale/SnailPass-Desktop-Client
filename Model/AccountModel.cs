using Newtonsoft.Json;

namespace SnailPass_Desktop.Model
{
    public class AccountModel
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string ServiceName { get; set; }

        [JsonProperty("login")]
        public string? Login { get; set; }

        [JsonProperty("encrypted_password")]
        public string Password { get; set; }

        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        [JsonProperty("is_favorite")]
        public string IsFavorite { get; set; }

        [JsonProperty("is_deleted")]
        public string IsDeleted { get; set; }

        [JsonProperty("creation_time")]
        public string CreationTime { get; set; }

        [JsonProperty("update_time")]
        public string UpdateTime { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        public AccountModel() { }

        public AccountModel(string iD, string serviceName, string? login, string password, 
            string nonce, string isFavorite, string isDeleted, string creationTime, 
            string updateTime, string userId)
        {
            ID = iD;
            ServiceName = serviceName;
            Login = login;
            Password = password;
            Nonce = nonce;
            IsFavorite = isFavorite;
            IsDeleted = isDeleted;
            CreationTime = creationTime;
            UpdateTime = updateTime;
            UserId = userId;
        }

        public override string ToString()
        {
            return $"ID: {ID}, Serivce name: {ServiceName}, Login: {Login}, Password(enc): {Password}, " +
                $"Nonce: {Nonce}, Is favorite: {IsFavorite}, Is deleted: {IsDeleted}, " +
                $"Creation time: {CreationTime}, Update time: {UpdateTime}, User ID: {UserId}";
        }
    }
}
