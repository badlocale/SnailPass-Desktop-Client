using Newtonsoft.Json;

namespace SnailPass_Desktop.Model
{
    public class CustomFieldModel
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("field_name")]
        public string FieldName { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        [JsonProperty("record_id")]
        public string AccountId { get; set; }

        public CustomFieldModel() { }

        public CustomFieldModel(string id, string fieldName, string value, string nonce, string accountId)
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
