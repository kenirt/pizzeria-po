using pizzeria.Interfaces;
using pizzeria.Models;


namespace pizzeria.Services
{
    public class SessionService(ILogger logger, IUserManager userManager)
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
        private readonly IUserManager _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager), "UserManager cannot be null.");
        private User? _currentUser;
        public User? CurrentUser => _currentUser;


        public void StartSession(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("Login or password cannot be empty.");
                throw new ArgumentException("Login and password must not be empty.");
            }

            var user = _userManager.AuthenticateUser(username, password);
            if (user == null)
            {
                _logger.LogWarning($"Authentication failed for user: {username}");
                throw new UnauthorizedAccessException("Invalid username or password.");
            }          
            
            _currentUser = user;
            _logger.LogInfo($"Session started for user: {user.Username}");
        }

        public void EndSession()
        {
            if (_currentUser != null)
            {
                _logger.LogInfo($"Session ended for user: {_currentUser.Username}");
                _currentUser = null;
            }
            else
            {
                _logger.LogWarning("No active session to end.");
            }
        }
    }
}