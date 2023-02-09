using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SnailPass_Desktop.ViewModel
{
    public class ErrorViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        protected delegate bool ValidationRule(string data);

        public bool HasErrors => _errors.Any();
        private Dictionary<string, List<string>> _errors = new();
        private Dictionary<string, List<ValidationRule>> _rules = new();

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return Enumerable.Empty<string>();
            }

            return _errors.GetValueOrDefault(propertyName, new List<string>());
        }

        public void AddError(string errorMessage, [CallerMemberName] string propertyName = null)
        {
            if (!_errors.ContainsKey(propertyName))
            {
                _errors.Add(propertyName, new List<string>());
            }

            _errors[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        public void ClearErrors([CallerMemberName] string propertyName = null)
        {
            _errors.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }

        protected void AddValidator(string? propertyName, ValidationRule validationRule)
        {

        }

        protected void Validate(string propertyName)
        {

        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
