using Serilog;
using SnailPass.Model;
using SnailPass.Model.Interfaces;
using SnailPass.ViewModel.Stores;
using System.Net;

namespace SnailPass.ViewModel.Commands
{
    public class EditNoteCommand : CommandBase
    {
        private NotesViewModel _viewModel;
        private ILogger _logger;
        private INoteRestApi _noteRestApi;
        private IUserIdentityStore _identity;
        private ICryptographyService _cryptographyService;
        private ISynchronizationService _synchronizationService;

        public EditNoteCommand(NotesViewModel viewModel,
            ILogger logger, INoteRestApi noteRestApi, IUserIdentityStore identity,
            ICryptographyService cryptographyService, ISynchronizationService synchronizationService)
        {
            _viewModel = viewModel;
            _logger = logger;
            _noteRestApi = noteRestApi;
            _identity = identity;
            _cryptographyService = cryptographyService;
            _synchronizationService = synchronizationService;
        }

        public override bool CanExecute(object? parameters)
        {
            NoteModel selected = _viewModel.SelectedNote;
            return (selected != null) && (selected.Name != _viewModel.NoteName || 
                                          selected.Content != _viewModel.NoteContent);
        }

        public async override void Execute(object? parameter)
        {
            NoteModel note = (NoteModel)_viewModel.SelectedNote.Clone();
            note.Name = _viewModel.NoteName;
            note.Content = _viewModel.NoteContent;

            _logger.Information($"Execute editing for note with ID: \"{note.ID}\".");

            await _cryptographyService.EncryptAsync(note);

            HttpStatusCode? code = await _noteRestApi.PutNoteAsync(note);
            if (code == HttpStatusCode.OK)
            {
                await _synchronizationService.SynchronizeAsync(_identity.CurrentUser.Email);
                await _viewModel.LoadNotesAsync();
            }
        }
    }
}
