using SnailPass.Model.Interfaces;
using SnailPass.Services;
using SnailPass.ViewModel.Stores;

namespace SnailPass.ViewModel.Commands
{
    public class ConnectCommand : CommandBase
    {
        IAuthenticationService _authenticationService;
        IUserIdentityStore _identity;

        public ConnectCommand(IAuthenticationService authenticationService, IUserIdentityStore identity)
        {
            _authenticationService = authenticationService;
            _identity = identity;
        }

        public override async void Execute(object? parameter)
        {
            LoggingResult loggingResult = await _authenticationService.LoginViaNetwork
                    (_identity.CurrentUser.Email, _identity.Master);
        }
    }
}
