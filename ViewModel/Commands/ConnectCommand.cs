using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.Services;
using SnailPass_Desktop.ViewModel.Stores;

namespace SnailPass_Desktop.ViewModel.Commands
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
