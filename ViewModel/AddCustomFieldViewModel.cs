using SnailPass_Desktop.Model;
using System;

namespace SnailPass_Desktop.ViewModel
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

                ClearErrors();
                if (string.IsNullOrWhiteSpace(_fieldName))
                {
                    AddError("Field name cannot be empty.");
                }
                if (_fieldName.Length != _fieldName.Trim().Length)
                {
                    AddError("Field name have leading or trailing white-spaces.");
                }
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();

                ClearErrors();
                if (string.IsNullOrWhiteSpace(_value))
                {
                    AddError("Value field cannot be empty.");
                }
                if (_value.Length != _value.Trim().Length)
                {
                    AddError("Value field have leading or trailing white-spaces.");
                }
            }
        }

        public AddCustomFieldViewModel()
        {
            _id = Guid.NewGuid().ToString();

            AddError("Please enter the field name", nameof(FieldName));
            AddError("Please enter the value of field", nameof(Value));
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
