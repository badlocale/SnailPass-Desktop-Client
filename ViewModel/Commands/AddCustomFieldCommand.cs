using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Services;
using SnailPass_Desktop.ViewModel.Stores;
using System.Net;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class AddCustomFieldCommand : CommandBase
    {
        private AccountsViewModel _viewModel;
        private ILogger _logger;
        private IDialogService _dialogService;
        private IUserIdentityStore _identity;
        private IRestClient _httpClient;
        private ISynchronizationService _synchronizationService;
        private ICryptographyService _cryptographyService;

        public AddCustomFieldCommand(AccountsViewModel viewModel, ILogger logger, 
            IDialogService dialogService, IUserIdentityStore identity, IRestClient httpClient, 
            ICryptographyService cryptographyService, ISynchronizationService synchronizationService)
        {
            _viewModel = viewModel;
            _logger = logger;
            _dialogService = dialogService;
            _identity = identity;
            _httpClient = httpClient;
            _cryptographyService = cryptographyService;
            _synchronizationService = synchronizationService;
        }

        public override async void Execute(object? parameter)
        {
            AddCustomFieldViewModel? dialogVM = _dialogService.ShowDialog<AddCustomFieldViewModel>();

            if (dialogVM != null)
            {
                EncryptableFieldModel model = dialogVM.CreateModel();

                _logger.Information($"Execute 'add new field' for account [{model.AccountId}] f" +
                    $"or e-mail [{_identity.CurrentUser.Email}]. Field name: [{model.FieldName}].");

                model.AccountId = _viewModel.SelectedAccount.ID;
                _cryptographyService.Encrypt(model);

                HttpStatusCode code = await _httpClient.PostCustomFieldAsync(model);
                if (code == HttpStatusCode.Created)
                {
                    await _synchronizationService.SynchronizeAsync(_identity.CurrentUser.Email);
                    _viewModel.LoadFields();
                }
            }
            else
            {
                _logger.Information("Dialog 'add new field' cancelled.");
            }
        }
    }
}
