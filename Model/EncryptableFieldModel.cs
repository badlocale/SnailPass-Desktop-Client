﻿using Newtonsoft.Json;
using SnailPass_Desktop.Model.Cryptography;

namespace SnailPass_Desktop.Model
{
    public class EncryptableFieldModel
    {
        private bool _isDeletable = true;

        [JsonProperty("id")]
        public string ID { get; set; }

        [CryptableField]
        [JsonProperty("field_name")]
        public string FieldName { get; set; }

        [CryptableField]
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("record_id")]
        public string AccountId { get; set; }

        public bool IsDeletable
        {
            get { return _isDeletable; }
            set { _isDeletable = value; }
        }

        public EncryptableFieldModel() { }

        public EncryptableFieldModel(string id, string fieldName, string value, string accountId)
        {
            ID = id;
            FieldName = fieldName;
            Value = value;
            AccountId = accountId;
        }

        public override string ToString()
        {
            return $"ID: {ID}, Field name: {FieldName}, Value: {Value}, Account ID: {AccountId}, IsDeletable: {IsDeletable}";
        }
    }
}