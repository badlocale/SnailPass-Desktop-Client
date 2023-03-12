namespace SnailPass_Desktop.Model.Interfaces
{
    public interface IUserRepository
    {
        bool AuthenticateLocally(string localKey, string username);
        void AddOrReplace(UserModel user);
        bool IsUsernameExist(string username);
        bool IsEmailExist(string email);
        UserModel? GetById(string id);
        UserModel? GetByEmail(string email);
    }
}
