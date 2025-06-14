using pizzeria.Interfaces;
using pizzeria.Models;
using pizzeria.Utils;
using System.Data.Common;
using System.Text.Json;

namespace pizzeria.Services
{
    public class UserManager : IUserManager
    {
        private readonly ILogger _logger;
        private readonly string _usersFilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "users.json");
        public List<User> Users { get; private set; } = new List<User>();

        private void LoadUsers()
        {
            if (!File.Exists(_usersFilePath))
            {
                _logger.LogWarning($"Users file not found at path: {_usersFilePath}. Creating a new one.");
                SaveUsers();
                return;
            }

            try
            {
                var json = File.ReadAllText(_usersFilePath);
                Users = JsonSerializer.Deserialize<List<User>>(json) ?? [];
                _logger.LogInfo($"Users loaded successfully from {_usersFilePath}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to load users from file: {ex.Message}");
                throw;
            }
        }
        private void SaveUsers()
        {
            try
            {
                var json = JsonSerializer.Serialize(Users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_usersFilePath, json);
                _logger.LogInfo($"Users saved successfully to {_usersFilePath}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save users to file: {ex.Message}");
                throw;
            }
        }
        
        public void CreateUser(string name, string username, string password)
        {
            var existingUser = Users.FirstOrDefault(u => u.Username == username);
            if (existingUser != null)
            {
                _logger.LogWarning($"User with username '{username}' already exists.");
                throw new InvalidOperationException($"User with username '{username}' already exists.");
            }
            Guid id = Guid.NewGuid();
            var user = new User
            {
                Username = username,
                Name = name,
                Password = HashPassword.Hash(password),
            };
            Users.Add(user);
            SaveUsers();
            _logger.LogInfo($"User created: ogin: {username}, name: {name}");
        }

        public void Initialize()
        {
            LoadUsers();
            _logger.LogInfo("UserManager initialized.");
        }

        public void Shutdown()
        {
            SaveUsers();
            _logger.LogInfo("UserManager shutdown. Users saved to file.");
        }

        public bool RemoveUserByUsername(string username)
        {
            var user = Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                Users.Remove(user);
                SaveUsers();
                _logger.LogInfo($"User removed: {user.Username}");
                return true;
            }
            _logger.LogWarning($"User not found for removal: {username}");
            return false;
        }

        public User? AuthenticateUser(string username, string password)
        {
            var user = Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                _logger.LogInfo($"Authentication failed for username: {username}. User not found.");
                return null;
            }

            if (user.Password != HashPassword.Hash(password))
            {
                _logger.LogInfo($"Authentication failed for username: {username}. Incorrect password.");
                return null;
            }

            _logger.LogInfo($"User authenticated successfully: {user.Username}, Login: {username}");
            return user;
        }

        public User? GetUserByUsername(string username)
        {
            var user = Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                _logger.LogWarning($"User with username '{username}' not found.");
                return null;
            }
            _logger.LogInfo($"User found: {user.Username}, Login: {username}");
            return user;
        }

        public UserManager(ILogger logger)
        {
            _logger = logger;
        }
    }
}