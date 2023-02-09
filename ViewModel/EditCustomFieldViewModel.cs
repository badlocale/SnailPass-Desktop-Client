using SnailPass_Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                ClearErrors();
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
                if (_value.Length != _value.Trim().Length)
                {
                    AddError("Value field have leading or trailing white-spaces.");
                }
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
