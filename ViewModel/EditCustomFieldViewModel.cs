using SnailPass_Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel
{
    public class EditCustomFieldViewModel : ViewModelBase
    {
        private string? _value;
        private string? _fieldName;

        private string _errorMessage = string.Empty;

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

        public EncryptableFieldModel CreateModel()
        {
            EncryptableFieldModel model = new();

            model.FieldName = _fieldName;
            model.Value = _value;

            return model;
        }
    }
}
