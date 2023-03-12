using SnailPass_Desktop.Model;
using System;
using System.Net.Mail;
using System.Net;
using Serilog;
using SnailPass_Desktop.Model.Interfaces;
using System.Configuration;
using SnailPass_Desktop.Model.Cryptography;
using System.Text;
using System.Security;
using SnailPass_Desktop.Services;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class RegistrationCommand : CommandBase
    {
        RegistrationViewModel _viewModel;
        IUserRestApi _userRestApi;
        IKeyGenerator _keyGenerator;
        ILogger _logger;
        IAuthenticationService _authenticationService;
        IDialogService _dialogService;

        public RegistrationCommand(RegistrationViewModel viewModel, IUserRestApi userRestApi,
            IKeyGenerator encryptor, ILogger logger, IAuthenticationService authenticationService, 
            IDialogService dialogService)
        {
            _viewModel = viewModel;
            _userRestApi = userRestApi;
            _keyGenerator = encryptor;
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