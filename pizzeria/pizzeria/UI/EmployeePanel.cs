namespace pizzeria.UI
{
    using System;
    using pizzeria.Interfaces;
    using pizzeria.Models;

    public partial class EmployeePanel
    {
        private readonly IOrderQueue _orderQueue;
        private readonly Menu _menu;
        private readonly ILogger _logger;
        public EmployeePanel(IOrderQueue orderQueue, Menu menu, ILogger logger)
        {
            _orderQueue = orderQueue ?? throw new ArgumentNullException(nameof(orderQueue), "OrderQueue cannot be null.");
            _menu = menu;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
        }

        private void ViewOrders()
        {
            Console.Clear();
            Console.WriteLine("Orders Menu:");
            Console.WriteLine("1. View Active Orders");
            Console.WriteLine("2. View Order History");
            Console.WriteLine("3. View User Orders");
            Console.WriteLine("Press any other key to return to the main menu.");

            var listOrders = new ListOrders(_orderQueue);
            _logger.LogInfo("Employee is viewing orders.");
            while (true)
            {
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        listOrders.ShowAllActiveOrders();
                        break;
                    case "2":
                        listOrders.ShowAllArchivedOrders();
                        break;
                    case "3":
                        Console.Write("Enter username to view orders: ");
                        var username = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(username))
                        {
                            Console.WriteLine("Username cannot be empty.");
                            _logger.LogWarning("Attempted to view orders with an empty username.");
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            continue;
                        }
                        listOrders.ShowUserActiveOrders(username);
                        listOrders.ShowUserArchivedOrders(username);
                        _logger.LogInfo($"Employee viewed orders for user: {username}");
                        break;
                    default:
                        return;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
        private void UpdateOrderStatus()
        {
            Console.Clear();
            Console.Write("Enter Order ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int orderId))
            {
                try
                {
                    _orderQueue.AdvanceOrderStatus(orderId);
                    Console.WriteLine($"Order {orderId} status updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating order status: {ex.Message}");
                    _logger.LogError($"Error updating order {orderId}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid Order ID.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void CancelOrder()
        {
            Console.Clear();
            Console.Write("Enter Order ID to cancel: ");
            if (int.TryParse(Console.ReadLine(), out int orderId))
            {
                try
                {
                    _orderQueue.CancelOrder(orderId);
                    Console.WriteLine($"Order {orderId} cancelled successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error cancelling order: {ex.Message}");
                    _logger.LogError($"Error cancelling order {orderId}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid Order ID.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void ManageOrders()
        {
            Console.Clear();
            Console.WriteLine("Order Management:");
            Console.WriteLine("1. View Orders");
            Console.WriteLine("2. Update Order Status");
            Console.WriteLine("3. Cancel Order");
            Console.WriteLine("Press any other key to return to the main menu.");

            while (true)
            {
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewOrders();
                        break;
                    case "2":
                        UpdateOrderStatus();
                        break;
                    case "3":
                        CancelOrder();
                        break;
                    default:
                        return;
                }
            }
        }
        public void Show()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Employee Panel!");
            Console.WriteLine("1. Manage Orders");
            Console.WriteLine("2. View Menu");
            Console.WriteLine("Press any other key to exit.");

            while (true)
            {
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageOrders();
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("Menu (Pizza, Base Price):");
                        foreach (var item in _menu.MenuItems)
                        {
                            Console.WriteLine($"{item.Name} - ${item.Price}");
                        }
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    default:
                        return;
                }
            }
        }
            
    }
}