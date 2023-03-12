using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface INoteRepository
    {
        public void AddOrReplace(NoteModel note);
        public void ReplaceAll(IEnumerable<NoteModel> notes);
        public IEnumerable<NoteModel> GetByUserId(string userId);
        public void DeleteAllByUsersEmail(string userId);
    }
}
