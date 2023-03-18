using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass.ViewModel
{
    public class TokenExpiredViewModel : ErrorViewModel
    {
        private SecureString _password;

        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public TokenExpiredViewModel()
        {
            AddValidationRule(nameof(Password), "Password field cannot be empty.", () =>
            {
                return !(_password?.Length == 0);
            });
            AddValidationRule(nameof(Password), "Password cannot be longer than 300 symbols.", () =>
            {
                return _password?.Length < 301;
            });

            Validate(null);
        }
    }
}
