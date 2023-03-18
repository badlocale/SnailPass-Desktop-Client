using Newtonsoft.Json;
using Serilog;
using SnailPass.Model;
using SnailPass.Model.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass.Data.API
{
    public class NoteApi : RestApiBase, INoteRestApi
    {
        public NoteApi(ILogger logger) : base(logger)
        {

        }

        public async Task<(HttpStatusCode?, IEnumerable<NoteModel>?)> GetNotesAsync()
        {
            try
            {
                ResponseMessage = await HttpClient.GetAsync("notes");
            }
            catch (HttpRequestException)
            {
                OnServerNotResponding();
                return (null, null);
            }

            IEnumerable<NoteModel>? notes = null;
            if (ResponseMessage.IsSuccessStatusCode)
            {
                string jsonString = ResponseMessage.Content.ReadAsStringAsync().Result;
                notes = JsonConvert.DeserializeObject<List<NoteModel>>(jsonString);
            }

            _logger.Information($"Getting notes status: {ResponseMessage.StatusCode}");

            if (notes == null)
            {
                return (ResponseMessage.StatusCode, new List<NoteModel>());
            }

            return (ResponseMessage.StatusCode, notes);
        }

        public async Task<HttpStatusCode?> DeleteNoteAsync(string noteId)
        {
            try
            {
                ResponseMessage = await HttpClient.DeleteAsync($"notes?id={noteId}");
            }
            catch (HttpRequestException)
            {
                OnServerNotResponding();
                return null;
            }

            _logger.Information($"Deleting note status: {ResponseMessage.StatusCode}");

            return ResponseMessage.StatusCode;
        }

        public async Task<HttpStatusCode?> PutNoteAsync(NoteModel note)
        {
            try
            {
                string jsonAccount = JsonConvert.SerializeObject(note);
                StringContent content = new StringContent(jsonAccount, Encoding.UTF8, "application/json");
                ResponseMessage = await HttpClient.PutAsync("notes", content);
            }
            catch (HttpRequestException)
            {
                OnServerNotResponding();
                return null;
            }
            _logger.Information($"Patching note status: {ResponseMessage?.StatusCode}");

            return ResponseMessage?.StatusCode;
        }

        public async Task<HttpStatusCode?> PostNoteAsync(NoteModel note)
        {
            try
            {
                string jsonNote = JsonConvert.SerializeObject(note);
                StringContent content = new StringContent(jsonNote, Encoding.UTF8, "application/json");
                ResponseMessage = await HttpClient.PostAsync("notes", content);
            }
            catch (HttpRequestException)
            {
                OnServerNotResponding();
                return null;
            }

            _logger.Information($"Posting note status: {ResponseMessage?.StatusCode}");

            return ResponseMessage?.StatusCode;
        }
    }
}
