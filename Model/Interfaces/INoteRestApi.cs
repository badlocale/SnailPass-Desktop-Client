using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface INoteRestApi
    {
        public Task<(HttpStatusCode?, IEnumerable<NoteModel>?)> GetNotesAsync();
        public Task<HttpStatusCode?> PostNoteAsync(NoteModel note);
        public Task<HttpStatusCode?> DeleteNoteAsync(string noteId);
        public Task<HttpStatusCode?> PutNoteAsync(NoteModel note);
    }
}
