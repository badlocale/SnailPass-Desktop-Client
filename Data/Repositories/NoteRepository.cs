using Microsoft.Data.Sqlite;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Interfaces;
using System;
using System.Collections.Generic;

namespace SnailPass_Desktop.Data.Repositories
{
    public class NoteRepository : RepositoryBase, INoteRepository
    {
        public void AddOrReplace(NoteModel note)
        {
            using var connection = GetConnection();
            using (SqliteCommand command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "REPLACE INTO notes (id, name, content, user_id, is_favorite, " +
                                                          "is_deleted, creation_time, update_time) " +
                                      "VALUES (@id, @name, @content, @user_id, " +
                                      "@is_favorite, @is_deleted, @creation_time, @update_time)";

                command.Parameters.Add("@id", SqliteType.Text).Value = note.ID;
                command.Parameters.Add("@name", SqliteType.Text).Value = note.Name;
                command.Parameters.Add("@content", SqliteType.Text).Value = note.Content;
                command.Parameters.Add("@user_id", SqliteType.Text).Value = note.UserId;
                command.Parameters.Add("@is_favorite", SqliteType.Text).Value = note.IsFavorite;
                command.Parameters.Add("@is_deleted", SqliteType.Text).Value = note.IsDeleted;
                command.Parameters.Add("@creation_time", SqliteType.Text).Value = note.CreationTime;
                command.Parameters.Add("@update_time", SqliteType.Text).Value = note.UpdateTime;

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<NoteModel> GetByUserId(string userId)
        {
            List<NoteModel> notes = new List<NoteModel>();

            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT id, name, content, is_favorite, is_deleted, " +
                                             "creation_time, update_time " +
                                      "FROM notes " +
                                      "WHERE user_id = @user_id AND is_deleted = 0;";

                command.Parameters.Add("@user_id", SqliteType.Text).Value = userId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NoteModel note = new NoteModel()
                        {
                            ID = reader[0].ToString(),
                            Name = reader[1].ToString(),
                            Content = reader[2].ToString(),
                            IsFavorite = Convert.ToBoolean(reader[3]),
                            IsDeleted = Convert.ToBoolean(reader[4]),
                            CreationTime = reader[5].ToString(),
                            UpdateTime = reader[6].ToString(),
                            UserId = userId
                        };

                        notes.Add(note);
                    }
                }
            }

            return notes;
        }

        public void DeleteAllByUsersEmail(string usersEmail)
        {
            using var connection = GetConnection();
            using (var command = new SqliteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE " +
                                      "FROM notes " +
                                      "WHERE notes.user_id IN (" +
                                          "SELECT user_id " +
                                          "FROM users " +
                                          "WHERE users.email = @email" +
                                      ");";

                command.Parameters.Add("@email", SqliteType.Text).Value = usersEmail;

                command.ExecuteNonQuery();
            }
        }
    }
}
