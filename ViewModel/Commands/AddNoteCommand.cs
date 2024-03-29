﻿using SnailPass.Model.Interfaces;
using SnailPass.Model;
using SnailPass.Services;
using SnailPass.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace SnailPass.ViewModel.Commands
{
    public class AddNoteCommand : CommandBase
    {
        private NotesViewModel _viewModel;
        private IDialogService _dialogService;
        private ILogger _logger;
        private INoteRestApi _noteRestApi;
        private IUserIdentityStore _identity;
        private ICryptographyService _cryptographyService;
        private ISynchronizationService _synchronizationService;

        public AddNoteCommand(NotesViewModel viewModel, IDialogService dialogService,
            ILogger logger, INoteRestApi noteRestApi, IUserIdentityStore identity,
            ICryptographyService cryptographyService, ISynchronizationService synchronizationService)
        {
            _viewModel = viewModel;
            _dialogService = dialogService;
            _logger = logger;
            _noteRestApi = noteRestApi;
            _identity = identity;
            _cryptographyService = cryptographyService;
            _synchronizationService = synchronizationService;
        }

        public override async void Execute(object? parameter)
        {
            AddNoteViewModel dialogVM = _dialogService.ShowDialog<AddNoteViewModel>();

            if (dialogVM != null)
            {
                _logger.Information($"Execute 'add new note' for user: \"{_identity.CurrentUser.Email}\".");

                NoteModel model = dialogVM.CreateModel();
                await _cryptographyService.EncryptAsync(model);
                HttpStatusCode? code = await _noteRestApi.PostNoteAsync(model);
                if (code == HttpStatusCode.Created)
                {
                    await _synchronizationService.SynchronizeAsync(_identity.CurrentUser.Email);
                    await _viewModel.LoadNotesAsync();
                }
            }
            else
            {
                _logger.Information("Dialog 'add new note' cancelled.");
            }
        }
    }
}
