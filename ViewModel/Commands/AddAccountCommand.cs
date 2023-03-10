using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.Services;
using SnailPass_Desktop.ViewModel.Stores;
using System.Net;
using System.Text;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class AddAccountCommand : CommandBase
    {
        private AccountsViewModel _viewModel;
        private IDialogService _dialogService;
        private ILogger _logger;
        private IAccountRestApi _accountsRestApi;
        private IUserIdentityStore _identity;
        private ICryptographyService _cryptographyService;
        private ISynchronizationService _synchronizationService;

        public AddAccountCommand(AccountsViewModel viewModel, IDialogService dialogService, 
            ILogger logger, IAccountRestApi accountRestApi, IUserIdentityStore identity, 
            ICryptographyService cryptographyService, ISynchronizationService synchronizationService)
        {
            _viewModel = viewModel;
            _dialogService = dialogService;
            _logger = logger;
            _accountsRestApi = accountRestApi;
            _identity = identity;
            _cryptographyService = cryptographyService;
            _synchronizationService = synchronizationService;   
        }

        public override async void Execute(object? parameter)
        {
            AddAccountViewModel dialogVM = _dialogService.ShowDialog<AddAccountViewModel>();

            if (dialogVM != null)
            {
                _logger.Information($"Execute 'add new account' for user: \"{_identity.CurrentUser.Email}\".");

                AccountModel model = dialogVM.CreateModel();
                _cryptographyService.Encrypt(model);
                HttpStatusCode? code = await _accountsRestApi.PostAccountAsync(model);
                if (code == HttpStatusCode.Created)
                {
                    await _synchronizationService.SynchronizeAsync(_identity.CurrentUser.Email);
                    _viewModel.LoadAccountsAsync();
                }
            }
            else
            {
                _logger.Information("Dialog 'add new account' cancelled.");
            }
        }
    }
}
