using Serilog;
using SnailPass_Desktop.Data;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Services;
using SnailPass_Desktop.ViewModel.Stores;
using System.Net;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class AddNewAccountCommand : CommandBase
    {
        private AccountsViewModel _viewModel;
        private IDialogService _dialogService;
        private ILogger _logger;
        private IRestClient _httpClient;
        private IUserIdentityStore _identity;
        private ICryptographyService _cryptographyService;
        private ISynchronizationService _synchronizationService;

        public AddNewAccountCommand(AccountsViewModel viewModel, IDialogService dialogService, 
            ILogger logger, IRestClient httpClient, IUserIdentityStore identity, 
            ICryptographyService cryptographyService, ISynchronizationService synchronizationService)
        {
            _viewModel = viewModel;
            _dialogService = dialogService;
            _logger = logger;
            _httpClient = httpClient;
            _identity = identity;
            _cryptographyService = cryptographyService;
            _synchronizationService = synchronizationService;   
        }

        public override async void Execute(object? parameter)
        {
            AddNewAccountViewModel dialogVM = _dialogService.ShowDialog<AddNewAccountViewModel>();

            if (dialogVM != null)
            {
                string email = _identity.CurrentUser.Email;
                _logger.Information($"Execute 'add new account' for user: \"{email}\".");

                AccountModel model = dialogVM.CreateModel();
                _cryptographyService.Encrypt(model);
                HttpStatusCode code = await _httpClient.PostAccountAsync(model);
                if (code == HttpStatusCode.Created)
                {
                    await _synchronizationService.SynchronizeAsync(email);
                    _viewModel.LoadAccounts();
                }
            }
            else
            {
                _logger.Information("Dialog 'add new account' cancelled.");
            }
        }
    }
}
