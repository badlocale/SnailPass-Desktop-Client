using Serilog;
using System;

namespace SnailPass_Desktop.ViewModel.Stores
{
    public class ApplicationModeStore : IApplicationModeStore
    {
        public event EventHandler LocalModeEnabled;
        public event EventHandler LocalModeDisabled;

        ILogger _logger;

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

        public ApplicationModeStore(ILogger logger)
        {
            _logger = logger;
        }

        private void OnModeChanged()
        {
            if (isLocalMode == true)
            {
                LocalModeEnabled?.Invoke(this, EventArgs.Empty);
                _logger.Debug("Application has been switched to local mode.");
            }
            else
            {
                LocalModeDisabled?.Invoke(this, EventArgs.Empty);
                _logger.Debug("Applicatio has been switched to network mode.");
            }
        }
    }
}
