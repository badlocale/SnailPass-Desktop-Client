using Serilog;
using SnailPass.Model;
using SnailPass.Model.Interfaces;
using SnailPass.Services;
using SnailPass.ViewModel.Stores;
using System.Net;
using System.Text;

namespace SnailPass.ViewModel.Commands
{
    public class AddFieldCommand : CommandBase
    {
        private AccountsViewModel _viewModel;
        private ILogger _logger;
        private IDialogService _dialogService;
        private IUserIdentityStore _identity;
        private ICustomFieldRestApi _customFieldRestApi;
        private ISynchronizationService _synchronizationService;
        private ICryptographyService _cryptographyService;

        public AddFieldCommand(AccountsViewModel viewModel, ILogger logger, 
            IDialogService dialogService, IUserIdentityStore identity, ICustomFieldRestApi customFieldRestApi, 
            ICryptographyService cryptographyService, ISynchronizationService synchronizationService)
        {
            _viewModel = viewModel;
            _logger = logger;
            _dialogService = dialogService;
            _identity = identity;
            _customFieldRestApi = customFieldRestApi;
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
                await _cryptographyService.EncryptAsync(model);

                HttpStatusCode? code = await _customFieldRestApi.PostCustomFieldAsync(model);
                if (code == HttpStatusCode.Created)
                {
                    if (await _synchronizationService.SynchronizeAsync(_identity.CurrentUser.Email) == false)
                    {
                        return;
                    }

                    await _viewModel.LoadFieldsAsync();
                }
            }
            else
            {
                _logger.Information("Dialog 'add new field' cancelled.");
            }
        }
    }
}
