using System.Threading.Tasks;

namespace SnailPass.Model.Interfaces
{
    public interface ISynchronizationService
    {
        public Task SynchronizeAsync(string email);
    } 
}
