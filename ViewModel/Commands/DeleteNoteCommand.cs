using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.ViewModel.Stores;
using System.Net;
using Serilog;

namespace SnailPass_Desktop.ViewModel.Commands
{
    public class DeleteNoteCommand : CommandBase
    {
        private NotesViewModel _viewModel;
        private INoteRestApi _noteRestApi;
        private ISynchronizationService _synchronizationService;
        private IUserIdentityStore _identity;
        private ILogger _logger;

        public DeleteNoteCommand(NotesViewModel viewModel, INoteRestApi noteRestApi,
            ISynchronizationService synchronizationService, IUserIdentityStore identity,
            ILogger logger)
        {
            _viewModel = viewModel;
            _noteRestApi = noteRestApi;
            _synchronizationService = synchronizationService;
            _identity = identity;
            _logger = logger;
        }

        public override bool CanExecute(object? parameters)
        {
            return _viewModel.SelectedNote != null;
        }

        public async override void Execute(object? parameter)
        {
            NoteModel note = _viewModel.SelectedNote;

            _logger.Information($"Execute deletion for note with ID: \"{note.ID}\".");

            HttpStatusCode? code = await _noteRestApi.DeleteNoteAsync(note.ID);
            if (code == HttpStatusCode.OK)
            {
                await _synchronizationService.SynchronizeAsync(_identity.CurrentUser.Email);
                _viewModel.LoadNotesAsync();
            }
        }
    }
}
