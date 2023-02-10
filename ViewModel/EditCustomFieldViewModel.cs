using SnailPass_Desktop.Model;

namespace SnailPass_Desktop.ViewModel
{
    public class EditCustomFieldViewModel : ErrorViewModel
    {
        private string? _value;
        private string? _fieldName;

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

        public EditCustomFieldViewModel()
        {
            //Field name validation
            AddValidationRule(nameof(FieldName), "Field name have leading or trailing white-spaces.", () =>
            {
                if (_fieldName == null) 
                    return true;
                return _fieldName.Length == _fieldName.Trim().Length; ;
            });
            AddValidationRule(nameof(FieldName), "Field name cannot be longer than 300 symbols.", () =>
            {
                if (_fieldName == null) 
                    return true;
                return _fieldName.Length < 301;
            });

            //Value validation
            AddValidationRule(nameof(Value), "Value field have leading or trailing white-spaces.", () =>
            {
                if (_value == null) 
                    return true;
                return _value.Length == _value.Trim().Length;
            });
            AddValidationRule(nameof(Value), "Value field cannot be longer than 300 symbols.", () =>
            {
                if (_value == null) 
                    return true;
                return _value.Length < 301;
            });

            Validate(null);
        }

        public EncryptableFieldModel CreateModel()
        {
            EncryptableFieldModel model = new();

            model.FieldName = _fieldName;
            model.Value = _value;

            return model;
        }
    }
}
