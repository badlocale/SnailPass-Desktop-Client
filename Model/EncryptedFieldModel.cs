using Newtonsoft.Json;
using SnailPass_Desktop.Model.Cryptography;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model
{
    public class EncryptedFieldModel
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [CryptableField]
        [JsonProperty("field_name")]
        public string FieldName { get; set; }

        [CryptableField]
        [JsonProperty("value")]
        public string Value { get; set; }

        [NonceField]
        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        [JsonProperty("record_id")]
        public string AccountId { get; set; }

        public EncryptedFieldModel() { }

        public EncryptedFieldModel(string id, string fieldName, string value, string nonce, string accountId)
        {
            ID = id;
            FieldName = fieldName;
            Value = value;
            Nonce = nonce;
            AccountId = accountId;
        }

        public override string ToString()
        {
            return $"ID: {ID}, Field name: {FieldName}, Value: {Value}, Nonce: {Nonce}, Account ID: {AccountId}";
        }
    }
}
