using Serilog;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Services;
using SnailPass_Desktop.ViewModel.Stores;
using System.Net;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class AddCustomFieldCommand : CommandBase
    {
        AccountsViewModel _viewModel;
        ILogger _logger;
        IDialogService _dialogService;
        IUserIdentityStore _identity;
        IRestClient _httpClient;

        public AddCustomFieldCommand(AccountsViewModel viewModel, ILogger logger, 
            IDialogService dialogService, IUserIdentityStore identity, IRestClient httpClient)
        {
            _viewModel = viewModel;
            _logger = logger;
            _dialogService = dialogService;
            _identity = identity;
            _httpClient = httpClient;
        }

        public override async void Execute(object? parameter)
        {
            AddCustonFieldViewModel vm = _dialogService.ShowDialog<AddCustonFieldViewModel>();

            if (vm != null)
            {
                string email = _identity.CurrentUser.Email;
                _logger.Information($"Execute 'add new field' for user: \"{email}\".");

                HttpStatusCode code = await _httpClient.PostCustomFieldAsync(vm.CreateModel()); 
                if (code == HttpStatusCode.Created)
                {
                    _viewModel.LoadCustomFields();
                }
            }
        }
    }
}
