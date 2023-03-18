using SnailPass.Model;
using System;

namespace SnailPass.ViewModel
{
    public class AddCustomFieldViewModel : ErrorViewModel
    {
        private string _id;
        private string _fieldName;
        private string _value;

        public string FieldName
        {
            get { return _fieldName; }
            set
            {
                _fieldName = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public AddCustomFieldViewModel()
        {
            _id = Guid.NewGuid().ToString();

            //Field name validation
            AddValidationRule(nameof(FieldName), "Field name cannot be empty.", () =>
            {
                return !string.IsNullOrEmpty(_fieldName);
            });
            AddValidationRule(nameof(FieldName), "Field name have leading or trailing white-spaces.", () =>
            {
                return _fieldName?.Length == _fieldName?.Trim().Length;
            });
            AddValidationRule(nameof(FieldName), "Field name cannot be longer than 300 symbols.", () =>
            {
                return _fieldName?.Length < 301;
            });


            //Value validation
            AddValidationRule(nameof(Value), "Value field cannot be empty.", () =>
            {
                return !string.IsNullOrEmpty(_value);
            });
            AddValidationRule(nameof(Value), "Value field have leading or trailing white-spaces.", () =>
            {
                return _value?.Length == _value?.Trim().Length;
            });
            AddValidationRule(nameof(Value), "Value field cannot be longer than 300 symbols.", () =>
            {
                return _value?.Length < 301;
            });

            Validate(null);
        }

        public EncryptableFieldModel? CreateModel()
        {
            EncryptableFieldModel model = new();

            model.ID = _id;
            model.FieldName = _fieldName;
            model.Value = _value;

            return model;
        }
    }
}
