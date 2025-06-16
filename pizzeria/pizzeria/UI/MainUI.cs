using pizzeria.Services;
using pizzeria.Interfaces;
using pizzeria.Models;
using pizzeria.Enums;

namespace pizzeria.UI
{
    public class MainUI
    {
        private readonly ILogger _logger;
        private readonly IUserManager _userManager;
        private readonly IOrderQueue _orderQueue;
        private readonly SessionService _sessionService;
        private readonly Menu _menu;

        public MainUI(ILogger logger, IUserManager userManager, IOrderQueue orderQueue, SessionService sessionService, Menu menu)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager), "UserManager cannot be null.");
            _orderQueue = orderQueue ?? throw new ArgumentNullException(nameof(orderQueue), "OrderQueue cannot be null.");
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService), "SessionService cannot be null.");
            _menu = menu ?? throw new ArgumentNullException(nameof(menu), "Menu cannot be null.");
        }

        public void Run()
        {
            _sessionService.StartSession("test", "#Test123"); // For testing purposes, replace with actual login logic
            //_sessionService.StartSession("employee", "employee"); // For testing purposes, replace with actual login logic
            while (true)
            {
                Console.WriteLine("Welcome to the Pizzeria Management System!");
                LoginPanel login = new(_logger, _userManager, _sessionService);
               // login.Show();


                if (_sessionService.CurrentUser == null)
                {
                    Console.WriteLine("Exiting...");
                    return;
                }
                Console.WriteLine($"Logged in as: {_sessionService.CurrentUser.Username}");

                switch (_sessionService.CurrentUser.Username)
                {
                    case "employee":
                        _logger.LogInfo("Employee log in.");
                        new EmployeePanel(_orderQueue, _menu, _logger).Show();
                        break;
                    default:
                        new UserPanel(_orderQueue, _sessionService.CurrentUser.Username, _menu).Show();
                        break;
                }

                _sessionService.EndSession();
                Console.WriteLine("Logged out successfully.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
