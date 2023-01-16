using Serilog;

namespace SnailPass_Desktop.ViewModel.Stores
{
    public class ApplicationModeStore : IApplicationModeStore
    {
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
                _logger.Debug("Application has been switched to local mode.");
            }
            else
            {
                _logger.Debug("Applicatio has been switched to network mode.");
            }
        }
    }
}
