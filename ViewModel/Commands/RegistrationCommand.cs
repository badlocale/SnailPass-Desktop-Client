using SnailPass.Model;
using System;
using System.Net.Mail;
using System.Net;
using Serilog;
using SnailPass.Model.Interfaces;
using SnailPass.Services;

namespace SnailPass.ViewModel.Commands
{
    public class RegistrationCommand : CommandBase
    {
        RegistrationViewModel _viewModel;
        ILogger _logger;
        IAuthenticationService _authenticationService;
        IDialogService _dialogService;

        public RegistrationCommand(RegistrationViewModel viewModel, ILogger logger, 
            IAuthenticationService authenticationService, IDialogService dialogService)
        {
            _viewModel = viewModel;
            _logger = logger;
            _authenticationService = authenticationService;
            _dialogService = dialogService;
        }

        public override async void Execute(object? obj)
        {
            _viewModel.ErrorMessage = string.Empty;
            _viewModel.ID = Guid.NewGuid().ToString();

            RegistrationResult result = await _authenticationService.Register(_viewModel.Email, 
                _viewModel.Password, _viewModel.Hint);

            if (result.IsSuccess)
            {
                _dialogService.ShowDialog("ResistrationSuccessDialog");
            }
            else
            {
                _viewModel.ErrorMessage = result.ErrorMessage ?? string.Empty;
            }
        }
    }
}