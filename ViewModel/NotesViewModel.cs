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
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SnailPass_Desktop.ViewModel
{
    public class NotesViewModel : ViewModelBase, IRefreshable
    {
        private ObservableCollection<NoteModel> _notes;
        private NoteModel? _selectedNote;
        private string _searchBarText = string.Empty;
        private bool _isNetworkFunctionsEnabled;
        private string? _noteName;
        private string? _noteContent;
        private string? _updationTime;
        private string? _creationTime;

        private INoteRepository _noteRepository;
        private IUserIdentityStore _identity;
        private ILogger _logger;
        private ICryptographyService _cryptographyService;

        public string SearchBarText
        {
            get { return _searchBarText; }
            set
            {
                _searchBarText = value;
                OnPropertyChanged();
                NotesCollectionView.Refresh();
            }
        }

        public NoteModel? SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                _selectedNote = value;

                if (_selectedNote != null)
                {
                    NoteName = _selectedNote.Name;
                    NoteContent = _selectedNote.Content;
                    UpdationTime = DateTime.Parse(_selectedNote.UpdateTime).ToShortDateString();
                    CreationTime = DateTime.Parse(_selectedNote.CreationTime).ToShortDateString();
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

        public string? NoteName
        {
            get { return _noteName; }
            set
            {
                _noteName = value;
                OnPropertyChanged();
            }
        }

        public string? NoteContent
        {
            get { return _noteContent; }
            set
            {
                _noteContent = value;
                OnPropertyChanged();
            }
        }

        public string? UpdationTime
        {
            get { return _updationTime; }
            set
            {
                _updationTime = value;
                OnPropertyChanged();
            }
        }

        public string? CreationTime
        {
            get { return _creationTime; }
            set
            {
                _creationTime = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddNoteCommand { get; set; }
        public ICommand DeleteNoteCommand { get; set; }
        public ICommand EditNoteCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

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
            RefreshCommand = new RefreshCommand(this, logger, identity, synchronizationService);

            NotesCollectionView = CollectionViewSource.GetDefaultView(_notes);
            NotesCollectionView.Filter = FilterWithSearchBar;

            NotesCollectionView = CollectionViewSource.GetDefaultView(_notes);

            applicationModeStore.LocalModeDisabled += LocalModeDisabledHandler;
            applicationModeStore.LocalModeEnabled += LocalModeEnabledHandler;

            LoadNotesAsync();
        }

        public async Task LoadNotesAsync()
        {
            IEnumerable<NoteModel> notes = _noteRepository.GetByUserId(_identity.CurrentUser.ID);

            List<Task> tasks = new();
            foreach (NoteModel note in notes)
            {
                tasks.Add(_cryptographyService.DecryptAsync(note));
            }
            await Task.WhenAll(tasks);

            _notes.Clear();
            foreach (NoteModel note in notes)
            {
                _notes.Add(note);
            }

            _logger.Information("Notes list loaded.");
        }

        private bool FilterWithSearchBar(object obj)
        {
            if (obj is NoteModel note)
            {
                return note.Name.Contains(SearchBarText);
            }
            return false;
        }

        private void LocalModeEnabledHandler(object? s, EventArgs args)
        {
            IsNetworkFunctionsEnabled = false;
        }

        private void LocalModeDisabledHandler(object? s, EventArgs args)
        {
            IsNetworkFunctionsEnabled = true;
        }

        public async Task RefreshAsync(object? args)
        {
            await LoadNotesAsync();
        }
    }
}
