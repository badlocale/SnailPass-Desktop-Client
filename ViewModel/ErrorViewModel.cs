using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SnailPass_Desktop.ViewModel
{
    public class ErrorViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public delegate bool ValidationCondition();

        public bool HasErrors
        {
            get { return _hasErrors; }
            set
            {
                _hasErrors = value;
                OnPropertyChanged();
            }
        }

        private bool _hasErrors = false;
        private Dictionary<string, List<string>> _errors = new();
        private Dictionary<string, List<(string, ValidationCondition)>> _checksMap = new();

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return Enumerable.Empty<string>();
            }

            return _errors.GetValueOrDefault(propertyName, new List<string>());
        }

        protected void AddValidationRule(string? propertyName, string errorMessage, ValidationCondition condition)
        {
            if (!_checksMap.ContainsKey(propertyName))
            {
                _checksMap.Add(propertyName, new List<(string, ValidationCondition)>());
            }

            _checksMap[propertyName].Add((errorMessage, condition));
        }

        protected void Validate([CallerMemberName] string? propertyName = null)
        {
            HasErrors = false;
            foreach (KeyValuePair<string, List<(string, ValidationCondition)>> pair in _checksMap)
            {
                string property = pair.Key;
                List<(string, ValidationCondition)> rules = pair.Value;

                if (propertyName == property)
                {
                    ClearErrors(property);
                }

                foreach ((string, ValidationCondition) rule in rules)
                {
                    string message;
                    ValidationCondition condition;
                    (message, condition) = rule;
                    bool isValid = condition.Invoke();
                    if (!isValid)
                    {
                        HasErrors = true;
                        if (propertyName == property)
                        {
                            AddError(message, property);
                        }
                    }
                }
            }
        }

        private void AddError(string errorMessage, string propertyName)
        {
            if (!_errors.ContainsKey(propertyName))
            {
                _errors.Add(propertyName, new List<string>());
            }

            _errors[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        private void ClearErrors(string propertyName)
        {
            _errors.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
