using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel
{
    public interface IRefreshable
    {
        public Task RefreshAsync(object? args);
    }
}
