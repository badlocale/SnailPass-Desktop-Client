using Serilog;
using SnailPass_Desktop.Data;
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

        public AddNewAccountCommand(AccountsViewModel viewModel, IDialogService dialogService, 
            ILogger logger, IRestClient httpClient, IUserIdentityStore identity)
        {
            _viewModel = viewModel;
            _dialogService = dialogService;
            _logger = logger;
            _httpClient = httpClient;
            _identity = identity;
        }

        public override async void Execute(object? parameter)
        {
            AddNewAccountViewModel vm = _dialogService.ShowDialog<AddNewAccountViewModel>();

            if (vm != null)
            {
                string email = _identity.CurrentUser.Email;
                _logger.Information($"Execute 'add new account' for user: \"{email}\".");

                HttpStatusCode code = await _httpClient.PostAccountAsync(vm.GetModel());
                if (code == HttpStatusCode.Created)
                {
                    _viewModel.LoadAccountListAsync();
                }
            }
            else
            {
                _logger.Information("Dialog 'add new account' cancelled.");
            }
        }
    }
}
