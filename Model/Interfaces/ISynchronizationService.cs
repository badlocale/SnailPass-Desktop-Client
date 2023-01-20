using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Interfaces
{
    public interface ISynchronizationService
    {
        public Task SynchronizeAsync(string email);
    } 
}
