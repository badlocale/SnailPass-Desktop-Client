using Serilog;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel
{
    public class AddNoteViewModel : ErrorViewModel
    {
        private string _id;
        private string _name;
        private string _content;
        private bool _isFavorite;
        private bool _isDeleted;
        private DateTime _creationTime;
        private DateTime _updateTime;
        private string _userId;

        private IUserIdentityStore _identity;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public AddNoteViewModel(IUserIdentityStore identity)
        {
            _identity = identity;

            _content = "PUDGE"; //
            _isFavorite = false;
            _isDeleted = false;
            _creationTime = DateTime.Now;
            _updateTime = DateTime.Now;
            _id = Guid.NewGuid().ToString();
            _userId = _identity.CurrentUser.ID;

            AddValidationRule(nameof(Name), "Note name field cannot be empty.", () =>
            {
                return !string.IsNullOrEmpty(_name);
            });
            AddValidationRule(nameof(Name), "Note name have leading or trailing white-spaces.", () =>
            {
                return _name?.Length == _name?.Trim().Length;
            });
            AddValidationRule(nameof(Name), "Note name cannot be longer than 50 symbols.", () =>
            {
                return _name?.Length < 51;
            });

            Validate(null);
        }

        public NoteModel CreateModel()
        {
            NoteModel noteModel = new NoteModel();

            noteModel.ID = _id;
            noteModel.Name = _name;
            noteModel.Content = _content;
            noteModel.UserId = _userId;
            noteModel.IsFavorite = _isFavorite;
            noteModel.IsDeleted = _isDeleted;
            noteModel.CreationTime = _creationTime.ToString();
            noteModel.UpdateTime = _updateTime.ToString();

            return noteModel;
        }
    }
}
