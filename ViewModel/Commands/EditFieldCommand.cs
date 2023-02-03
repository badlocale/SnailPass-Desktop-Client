using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Services;
using SnailPass_Desktop.ViewModel.Stores;
using System.Net;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class EditFieldCommand : CommandBase
    {
        private AccountsViewModel _viewModel;
        private ILogger _logger;
        private IDialogService _dialogService;
        private IUserIdentityStore _identity;
        private ICustomFieldRestApi _customFieldRestApi;
        private ISynchronizationService _synchronizationService;
        private ICryptographyService _cryptographyService;

        public EditFieldCommand(AccountsViewModel viewModel, ILogger logger,
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

        public async override void Execute(object? parameter)
        {
            EncryptableFieldModel selectedField = _viewModel.SelectedField;
            EncryptableFieldModel newData;

            AddCustomFieldViewModel? dialogVM = _dialogService.ShowDialog<AddCustomFieldViewModel>();

            if (dialogVM != null)
            {
                newData = dialogVM.CreateModel();

                _logger.Information($"Execute 'edit field data' for account [{selectedField.AccountId}]" +
                    $"for e-mail [{_identity.CurrentUser.Email}]. Old field name: [{selectedField.FieldName}].");

                if (!string.IsNullOrEmpty(newData.FieldName))
                {
                    selectedField.FieldName = newData.FieldName;
                }

                if (!string.IsNullOrEmpty(newData.Value))
                {
                    selectedField.Value = newData.Value;
                }

                _cryptographyService.Encrypt(selectedField);

                HttpStatusCode? code = await _customFieldRestApi.PatchCustomFieldAsync(selectedField);
                if (code == HttpStatusCode.OK)
                {
                    await _synchronizationService.SynchronizeAsync(_identity.CurrentUser.Email);
                    _viewModel.LoadFields();
                }
            }
            else
            {
                _logger.Information("Dialog 'edit field data' cancelled.");
            }
        }
    }
}
