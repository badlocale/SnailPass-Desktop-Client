using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.Services;
using SnailPass_Desktop.ViewModel.Commands;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace SnailPass_Desktop.ViewModel
{
    public class NotesViewModel : ViewModelBase
    {
        private ObservableCollection<NoteModel> _notes;
        private NoteModel _selectedNote;
        private bool _isNetworkFunctionsEnabled;
        private string _noteName;
        private string _noteContent;

        private INoteRepository _noteRepository;
        private IUserIdentityStore _identity;
        private ILogger _logger;
        private ICryptographyService _cryptographyService;

        public NoteModel SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                _selectedNote = value;

                if (value != null)
                {
                    NoteName = value.Name;
                    NoteContent = value.Content;
                }
                else
                {
                    NoteName = "";
                    NoteContent = "";
                }

                OnPropertyChanged();
            }
        }

        public bool IsNetworkFunctionsEnabled
        {
            get { return _isNetworkFunctionsEnabled; }
            set
            {
                _isNetworkFunctionsEnabled = value;
                OnPropertyChanged();
            }
        }

        public string NoteName
        {
            get { return _noteName; }
            set
            {
                _noteName = value;
                OnPropertyChanged();
            }
        }

        public string NoteContent
        {
            get { return _noteContent; }
            set
            {
                _noteContent = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddNoteCommand { get; set; }
        public ICommand DeleteNoteCommand { get; set; }
        public ICommand EditNoteCommand { get; set; }

        public ICollectionView NotesCollectionView { get; }

        public NotesViewModel(IUserIdentityStore identity, INoteRepository noteRepository,
            ICryptographyService cryptographyService, ILogger logger, IApplicationModeStore applicationModeStore,
            IDialogService dialogService, INoteRestApi noteRestApi, ISynchronizationService synchronizationService)
        {
            _noteRepository = noteRepository;
            _identity = identity;
            _logger = logger;
            _cryptographyService = cryptographyService;

            _notes = new ObservableCollection<NoteModel>();
            _isNetworkFunctionsEnabled = !applicationModeStore.IsLocalMode;

            AddNoteCommand = new AddNoteCommand(this, dialogService, logger, noteRestApi,
                identity, cryptographyService, synchronizationService);
            DeleteNoteCommand = new DeleteNoteCommand(this, noteRestApi, synchronizationService,
                identity, logger);
            EditNoteCommand = new EditNoteCommand(this, logger, noteRestApi, identity, 
                cryptographyService, synchronizationService);

            NotesCollectionView = CollectionViewSource.GetDefaultView(_notes);

            applicationModeStore.LocalModeDisabled += LocalModeDisabledHandler;
            applicationModeStore.LocalModeEnabled += LocalModeEnabledHandler;

            LoadNotes();
        }

        public void LoadNotes()
        {
            IEnumerable<NoteModel> notes = _noteRepository.GetByUserId(_identity.CurrentUser.ID);

            _notes.Clear();

            foreach (NoteModel note in notes)
            {
                _cryptographyService.Decrypt(note);
                _notes.Add(note);
            }

            _logger.Information("Notes list loaded.");
        }

        private void LocalModeEnabledHandler(object? s, EventArgs args)
        {
            IsNetworkFunctionsEnabled = false;
        }

        private void LocalModeDisabledHandler(object? s, EventArgs args)
        {
            IsNetworkFunctionsEnabled = true;
        }
    }
}
