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
    }
}
