using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.Services;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class LoginCommand : CommandBase
    {
        private LoginViewModel _viewModel;
        private IAuthenticationService _authenticationService;

        public LoginCommand(LoginViewModel viewModel, IAuthenticationService authenticationService)
        {
            _viewModel = viewModel;
            _authenticationService = authenticationService;
        }

        public override async void Execute(object? parameter)
        {
            _viewModel.ErrorMessage = string.Empty;

            LoggingResult result = await _authenticationService.Login(_viewModel.Email, _viewModel.Password);

            if (result.IsSuccess && result.User != null)
            {
                _viewModel.IsViewVisible = false;
            }
            else
            {
                _viewModel.ErrorMessage = result.ErrorMessage ?? string.Empty;
            }
        }
    }
}
