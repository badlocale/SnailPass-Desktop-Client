using Microsoft.Data.Sqlite;
using SnailPass.Model;
using SnailPass.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnailPass.Data.Repositories
{
    public class NoteRepository : RepositoryBase, INoteRepository
    {
        public void ReplaceAll(IEnumerable<NoteModel> notes, string usersEmail)
        {
            using SqliteConnection connection = GetConnection();
            connection.Open();
            using (SqliteTransaction transaction = connection.BeginTransaction())
            {
                using (SqliteCommand deleteCommand = connection.CreateCommand())
                {
                    deleteCommand.CommandText = "DELETE " +
                                                "FROM notes " +
                                                "WHERE notes.user_id IN (" +
                                                    "SELECT user_id " +
                                                    "FROM users " +
                                                    "WHERE users.email = @email" +
                                                ");";
                    deleteCommand.Parameters.Add("@email", SqliteType.Text).Value = usersEmail;
                    deleteCommand.ExecuteNonQuery();
                }

                if (notes.Count() > 0)
                {
                    using (SqliteCommand insertCommand = connection.CreateCommand())
                    {
                        StringBuilder sb = new();
                        sb.Append("INSERT INTO notes (id, name, content, user_id, is_favorite, " +
                                  "is_deleted, creation_time, update_time) " +
                                  "VALUES");
                        foreach (NoteModel note in notes)
                        {
                            sb.Append($"('{note.ID}', '{note.Name}', '{note.Content}', " +
                                $"'{note.UserId}', '{note.IsFavorite}', '{note.IsDeleted}', " +
                                $"'{note.CreationTime}', '{note.UpdateTime}'),");
                        }
                        sb.Remove(sb.Length - 1, 1).Append(";");
                        insertCommand.CommandText = sb.ToString();

                        insertCommand.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
        }

        public void AddOrReplace(NoteModel note)
        {
            using var connection = GetConnection();
            using (SqliteCommand command = new())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "REPLACE INTO notes (id, name, content, user_id, is_favorite, " +
                                                          "is_deleted, creation_time, update_time) " +
                                      "VALUES (@id, @name, @content, @user_id, " +
                                      "@is_favorite, @is_deleted, @creation_time, @update_time);";

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
                                      "WHERE user_id = @user_id AND is_deleted = 'False';";

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
