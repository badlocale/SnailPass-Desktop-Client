using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.ViewModel.Stores;
using System.Net;
using SnailPass_Desktop.Model.Interfaces;
using System.Net.Http;
using SnailPass_Desktop.Model.Cryptography;
using SnailPass_Desktop.Services;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class LoginCommand : CommandBase
    {
        private LoginViewModel _viewModel;
        private IUserIdentityStore _identity;
        private IAuthenticationService _authenticationService;

        public LoginCommand(LoginViewModel viewModel, IUserIdentityStore identity, 
            IAuthenticationService authenticationService)
        {
            _viewModel = viewModel;
            _identity = identity;
            _authenticationService = authenticationService;
        }

        public override async void Execute(object? parameter)
        {
            _viewModel.ErrorMessage = string.Empty;

            LoggingResult result = await _authenticationService.Login(_viewModel.Email, _viewModel.Password);

            if (result.IsSuccess && result.User != null)
            {
                _identity.CurrentUser = result.User;
                _identity.Master = _viewModel.Password;
                _viewModel.IsViewVisible = false;
            }
            else
            {
                _viewModel.ErrorMessage = result.ErrorMessage ?? string.Empty;
            }
        }
    }
}
