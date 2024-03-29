﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass.Model.Interfaces
{
    public interface INoteRepository
    {
        public void AddOrReplace(NoteModel note);
        public void ReplaceAll(IEnumerable<NoteModel> notes, string email);
        public IEnumerable<NoteModel> GetByUserId(string userId);
        public void DeleteAllByUsersEmail(string userId);
    }
}
