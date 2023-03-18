using Serilog;
using SnailPass.Data.API;
using SnailPass.Model.Interfaces;
using System;

namespace SnailPass.ViewModel.Stores
{
    public class ApplicationModeStore : IApplicationModeStore
    {
        public event EventHandler LocalModeEnabled;
        public event EventHandler LocalModeDisabled;

        private ILogger _logger;

        private bool isLocalMode = false;

        public bool IsLocalMode
        {
            get { return isLocalMode; }
            set 
            {
                isLocalMode = value;
                OnModeChanged();
            }
        }

        public ApplicationModeStore(ILogger logger, IAuthenticationService authenticationService)
        {
            _logger = logger;

            RestApiBase.ServerNotResponding += ServerNotRespondingHandler;
            authenticationService.LoggedLocally += LoggedLocallyHandler;
            authenticationService.LoggedViaNetwork += LoggedViaNetworkHandler;
        }

        private void OnModeChanged()
        {
            if (isLocalMode == true)
            {
                OnLocalModeEnabled();
                _logger.Debug("Application has been switched to local mode.");
            }
            else
            {
                OnLocalModeDisabled();
                _logger.Debug("Applicatio has been switched to network mode.");
            }
        }

        private void OnLocalModeEnabled()
        {
            LocalModeEnabled?.Invoke(this, EventArgs.Empty);
        }

        private void OnLocalModeDisabled()
        {
            LocalModeDisabled?.Invoke(this, EventArgs.Empty);
        }

        private void ServerNotRespondingHandler(object? sender, EventArgs args)
        {
            IsLocalMode = true;
        }

        private void LoggedLocallyHandler(object? sender, EventArgs args)
        {
            IsLocalMode = true;
        }

        private void LoggedViaNetworkHandler(object? sender, EventArgs args)
        {
            IsLocalMode = false;
        }
    }
}
