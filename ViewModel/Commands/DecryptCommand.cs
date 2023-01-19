using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Cryptography;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class DecryptPasswordCommand : CommandBase
    {
        private AccountsViewModel _viewModel;
        private ISymmetricCryptographer _cryptohrapher;
        private IMasterPasswordEncryptor _encryptor;
        private ILogger _logger;
        private IUserIdentityStore _identity;

        public DecryptPasswordCommand(AccountsViewModel viewModel, ISymmetricCryptographer cryptographer, 
            IMasterPasswordEncryptor encryptor, ILogger logger, IUserIdentityStore identity)
        {
            _viewModel = viewModel;
            _cryptohrapher = cryptographer;
            _encryptor = encryptor;
            _logger = logger;
            _identity = identity;
        }

        public override void Execute(object? parameter)
        {
            AccountModel account = _viewModel.SelectedAccount;

            if (account != null && !account.IsPasswordDecrypted)
            {
                _logger.Information("Executed dectypt command.");

                string value = account.Password;
                string nonce = account.Nonce;
                string encKey = _encryptor.Encrypt(_identity.Master, _identity.CurrentUser.Email, CryptoConstants.ENC_KEY_ITERATIONS_COUNT);
                account.SetDecryptedPass(_cryptohrapher.Decrypt(value, encKey, nonce));
                _viewModel.OnPropertyChanged(nameof(_viewModel.SelectedAccount));
            }
        }
    }
}
