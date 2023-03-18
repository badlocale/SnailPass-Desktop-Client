using System.Threading.Tasks;

namespace SnailPass.ViewModel
{
    public interface IRefreshable
    {
        public Task RefreshAsync(object? args);
    }
}
