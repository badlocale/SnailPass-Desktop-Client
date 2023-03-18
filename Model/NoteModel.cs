using Newtonsoft.Json;
using SnailPass.Model.Cryptography;
using System;

namespace SnailPass.Model
{
    public class NoteModel : ICloneable
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [CryptableField]
        [JsonProperty("name")]
        public string Name { get; set; }

        [CryptableField]
        [JsonProperty("content")]
        public string Content { get; set; }

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

        public NoteModel() { }

        public NoteModel(string id, string name, string content, bool isFavorite, bool isDeleted, 
            string creationTime, string updateTime, string userId)
        {
            ID = id;
            Name = name;
            Content = content;
            IsFavorite = isFavorite;
            IsDeleted = isDeleted;
            CreationTime = creationTime;
            UpdateTime = updateTime;
            UserId = userId;
        }

        public override string ToString()
        {
            return $"ID: {ID}, Name: {Name}, Content: {Content}, Is favorite: {IsFavorite}, " +
                $"Is deleted: {IsDeleted}, Creation time: {CreationTime}, Update time: {UpdateTime}, User ID: {UserId}";
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}