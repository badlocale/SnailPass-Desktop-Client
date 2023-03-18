using System;
using AutoUpdaterDotNET;
using Serilog;
using SnailPass.Services;
using System.Windows;
using System.Configuration;
using System.Threading.Tasks;

namespace SnailPass.Services
{
    public class UpdationService : IUpdationService
    {
        private ILogger _logger;
        private IDialogService _dialogService;

        public UpdationService(ILogger logger, IDialogService dialogService)
        {
            _logger = logger;
            _dialogService = dialogService;

            AutoUpdater.CheckForUpdateEvent += CheckForUpdateEventHandler;
        }

        public async void StartAsync()
        {
            Task delay = Task.Delay(500); //Del

            string? url = ConfigurationManager.AppSettings["update_xml_url"];

            if (url == null)
            {
                _logger.Fatal("Config file does not contain update file URL.");
                throw new ConfigurationErrorsException("Cant find update file URL in config file.");
            }

            await delay;

            AutoUpdater.Start(url);
        }

        private void CheckForUpdateEventHandler(UpdateInfoEventArgs args)
        {
            if (args.Error == null)
            {
                bool? dialogResult;
                if (args.IsUpdateAvailable)
                {
                    if (args.Mandatory.Value)
                    {
                        dialogResult = _dialogService.ShowDialog("ImportantUpdateAppDialog");
                    }
                    else
                    {
                        dialogResult = _dialogService.ShowDialog("UpdateAppDialog");
                    }

                    if (dialogResult == true)
                    {
                        try
                        {
                            if (AutoUpdater.DownloadUpdate(args))
                            {
                                Application.Current.Shutdown();
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.Error(e, "Cant download application update.");
                        }
                    }
                }
            }
        }
    }
}
