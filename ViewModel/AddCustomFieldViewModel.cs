using SnailPass_Desktop.Model;
using System;

namespace SnailPass_Desktop.ViewModel
{
    public class AddCustomFieldViewModel : ViewModelBase
    {
        private string _id;
        private string _fieldName;
        private string _value;

        private string _errorMessage;

        public string FieldName
        {
            get { return _fieldName; }
            set
            {
                _fieldName = value;
                OnPropertyChanged();
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public AddCustomFieldViewModel()
        {
            _id = Guid.NewGuid().ToString();
        }

        public EncryptableFieldModel CreateModel()
        {
            EncryptableFieldModel model = new();

            model.ID = _id;
            model.AccountId = null;
            model.FieldName = _fieldName;
            model.Value = _value;

            return model;
        }
    }
}
