using pizzeria.Services;
using pizzeria.Interfaces;
using pizzeria.Utils;

namespace pizzeria.UI;
public class LoginPanel
{
    private readonly ILogger _logger;
    private readonly IUserManager _userManager;
    private readonly SessionService _sessionService;
    private bool LoggedIn = false;
    public LoginPanel(ILogger logger, IUserManager userManager, SessionService sessionService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager), "UserManager cannot be null.");
        _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService), "SessionService cannot be null.");
    }

    private void Login()
    {
        Console.Write("Username: ");
        var username = Console.ReadLine();
        
        Console.Write("Password: ");
        var password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Login and password must not be empty. Please try again.");
            _logger.LogWarning("Login or password was empty during username attempt.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        try
        {
            _sessionService.StartSession(username, password);
            LoggedIn = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login failed: {ex.Message}");
            _logger.LogError($"Login failed for user '{username}': {ex.Message}");
            Console.WriteLine("Please try again.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private void Register()
    {
        Console.Write("Enter a username: ");
        var username = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(username) || !Validator.IsValidLogin(username))
        {
            Console.WriteLine("Incorrect username. Username must be at least 3 characters long and contain only letters, digits, and underscores.");
            _logger.LogWarning($"Register: Invalid username attempt: '{username}'");
            Console.WriteLine("Please try again.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }
        if (_userManager.GetUserByUsername(username) != null)
        {
            Console.WriteLine($"Username '{username}' is already taken. Please choose a different username.");
            _logger.LogWarning($"Register: Username '{username}' already exists.");
            Console.WriteLine("Please try again.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter a password: ");
        var password = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(password) || !Validator.IsValidPassword(password))
        {
            Console.WriteLine("Incorrect password. Password must be at least 6 characters long, contain at least one uppercase letter, \none lowercase letter, one digit, and one special character.");
            _logger.LogWarning($"Register: Invalid password attempt for user '{username}': '{password}'");
            Console.WriteLine("Please try again.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter your name: ");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name) || !Validator.IsValidName(name))
        {
            Console.WriteLine("Incorrect name. Name must not be empty and must be at most 20 characters long.");
            _logger.LogWarning($"Register: Invalid name attempt for user '{username}': '{name}'");
            Console.WriteLine("Please try again.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        try
        {
            _userManager.CreateUser(name, username, password);
            Console.WriteLine("Registration successful!");
            _logger.LogInfo($"User registered: {username}");
            Console.WriteLine("You can now log in with your new account.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Registration failed: {ex.Message}");
            _logger.LogError($"Registration failed for user '{username}': {ex.Message}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        return;
    }
    public void Show()
    {
        while (!LoggedIn)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Pizzeria Management System!");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("Press any other key to exit");
            Console.Write("Select an option: ");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Login();
                    break;
                case "2":
                    Register();
                    break;
                default:
                    return;
            }
        }
    }
}