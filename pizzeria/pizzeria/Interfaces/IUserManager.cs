using pizzeria.Models;

namespace pizzeria.Interfaces
{
    public interface IUserManager
    {
        List<User> Users { get; }
        void Initialize();
        void Shutdown();
        void CreateUser(string name, string username, string password);
        bool RemoveUserByUsername(string username);
        User? GetUserByUsername(string login);
        User? AuthenticateUser(string login, string password);
    }
}
