using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Net;
using System.Threading;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class DeleteFieldCommand : CommandBase
    {
        private AccountsViewModel _viewModel;
        private ICustomFieldRestApi _customFieldRestApi;
        private ISynchronizationService _synchronizationService;
        private IUserIdentityStore _identity;
        private ILogger _logger;

        public DeleteFieldCommand(AccountsViewModel viewModel, ICustomFieldRestApi customFieldRestApi,
            ISynchronizationService synchronizationService, IUserIdentityStore identity,
            ILogger logger)
        {
            _viewModel = viewModel;
            _customFieldRestApi = customFieldRestApi;
            _synchronizationService = synchronizationService;
            _identity = identity;
            _logger = logger;
        }

        public async override void Execute(object? parameter)
        {
            EncryptableFieldModel field = _viewModel.SelectedField;

            //CanExecute works not correct in this case
            if (field == null || !field.IsDeletable)
            {
                return;
            }

            _logger.Information($"Execute deletion for field with ID: \"{field.ID}\".");

            HttpStatusCode? code = await _customFieldRestApi.DeleteCustomFieldAsync(field.ID);
            if (code == HttpStatusCode.OK)
            {
                await _synchronizationService.SynchronizeAsync(_identity.CurrentUser.Email);
                await _viewModel.LoadFieldsAsync();
            }
        }
    }
}
