using pizzeria.Interfaces;
using pizzeria.Enums;
using pizzeria.Models;
using System.Text.Json;

namespace pizzeria.Services
{
    public class OrderQueue : IOrderQueue
    {
        public List<Order> ActiveOrders { get; private set; } = [];
        public List<OrderArchiveSnapshot> ArchivedOrders { get; private set; } = [];
        private readonly IPromotionManager _promotionManager;
        private readonly ILogger _logger;
        private readonly string _activeOrdersPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "active_orders.json");
        private readonly string _archivedOrdersPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "archived_orders.json");

        public OrderQueue(IPromotionManager promotionManager, ILogger logger)
        {
            _promotionManager = promotionManager ?? throw new ArgumentNullException(nameof(promotionManager), "Promotion manager cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
        }

        private int GetNextAvailableOrderId()
        {
            if (ActiveOrders.Count == 0)
                return 1;

            var usedIds = new HashSet<int>(ActiveOrders.Select(o => o.Id));
            for (int i = 1; i <= usedIds.Max(); i++)
            {
                if (!usedIds.Contains(i))
                    return i;
            }
            return usedIds.Max() + 1;
        }

        private void LoadOrdersFromFile()
        {
            if (File.Exists(_activeOrdersPath) && new FileInfo(_activeOrdersPath).Length > 0)
            {
                try
                {
                    var json = File.ReadAllText(_activeOrdersPath);
                    ActiveOrders = JsonSerializer.Deserialize<List<Order>>(json) ?? throw new InvalidOperationException("Failed to deserialize active orders from file.");
                    _logger.LogInfo($"Active orders loaded successfully from {_activeOrdersPath}.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to load active orders from file: {ex.Message}");
                    ActiveOrders = [];
                }
            }

            if (File.Exists(_archivedOrdersPath) && new FileInfo(_archivedOrdersPath).Length > 0)
            {
                try
                {
                    var json = File.ReadAllText(_archivedOrdersPath);
                    ArchivedOrders = JsonSerializer.Deserialize<List<OrderArchiveSnapshot>>(json) ?? throw new InvalidOperationException("Failed to deserialize archived orders from file.");
                    _logger.LogInfo($"Archived orders loaded successfully from {_archivedOrdersPath}.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to load archived orders from file: {ex.Message}");
                    ArchivedOrders = [];
                }
            }
        }
        private void SaveOrdersToFile()
        {
            try
            {
                var activeOrdersJson = JsonSerializer.Serialize(ActiveOrders, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_activeOrdersPath, activeOrdersJson);
                _logger.LogInfo($"Active orders saved successfully to {_activeOrdersPath}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save active orders to file: {ex.Message}");
            }

            try
            {
                var archivedOrdersJson = JsonSerializer.Serialize(ArchivedOrders, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_archivedOrdersPath, archivedOrdersJson);
                _logger.LogInfo($"Archived orders saved successfully to {_archivedOrdersPath}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save archived orders to file: {ex.Message}");
            }
        }
        public void Initialize()
        {
            LoadOrdersFromFile();
            _logger.LogInfo("OrderQueue initialized.");
        }
        public void Shutdown()
        {
            SaveOrdersToFile();
            _logger.LogInfo("OrderQueue shutdown. Active orders saved to file.");
        }

        public int PlaceOrder(string username, List<IPizza> pizzas)
        {
            if (pizzas == null || pizzas.Count == 0)
                throw new ArgumentException("Order must contain at least one pizza.", nameof(pizzas));

            var pizzaSnapshots = pizzas.Select(p =>
                p is CustomPizza customPizza
                    ? new OrderPizzaSnapshot
                    {
                        Name = customPizza.Name,
                        Size = customPizza.Size.ToString(),
                        Price = customPizza.CalculatePrice(),
                        Ingredients = [.. customPizza.Ingredients.Select(i => i.Name)],
                    }
                    : new OrderPizzaSnapshot
                    {
                        Name = p.Name,
                        Size = p.Size.ToString(),
                        Price = p.CalculatePrice(),
                    }
                ).ToList();

            var order = new Order
            {
                Id = GetNextAvailableOrderId(),
                Username = username,
                Pizzas = pizzaSnapshots,
                InitialPrice = pizzas.Sum(p => p.CalculatePrice()),
                IsFirstOrder = !HasUserMadeAnyOrders(username),
            };

            var (Name, Discount) = _promotionManager.GetBestPromotion(order);
            if (Name != null)
            {
                order.SetPromotionName(Name);
                order.SetFinalPrice(order.InitialPrice - Discount);
            }
            else
            {
                order.SetFinalPrice(order.InitialPrice);
            }

            ActiveOrders.Add(order);
            _logger.LogInfo($"Order {order.Id} placed by user {username} with {pizzas.Count} pizzas.");
            return order.Id;
        }
        public void CancelOrder(int orderId)
        {
            var order = ActiveOrders.FirstOrDefault(o => o.Id == orderId) ?? throw new ArgumentException($"Order with ID {orderId} not found.");
            if (order.Status != OrderStatus.Pending)
                throw new InvalidOperationException("Cannot cancel an order that is not pending.");

            order.SetStatus(OrderStatus.Cancelled);
            _logger.LogInfo($"Order {order.Id} has been cancelled.");
            ArchivedOrders.Add(new OrderArchiveSnapshot
            {
                StatusHistory = order.StatusHistory,
                Username = order.Username,
                Pizzas = order.Pizzas,
                InitialPrice = order.InitialPrice,
                FinalPrice = order.FinalPrice,
                PromotionName = order.PromotionName
            });
            ActiveOrders.Remove(order);
        }

        public void CancelAllOrders()
        {
            foreach (var order in ActiveOrders)
            {
                CancelOrder(order.Id);
            }
            ActiveOrders.Clear();
            _logger.LogWarning("All orders have been cancelled.");
        }

        public void ClearOrderHistory()
        {
            ArchivedOrders.Clear();
            _logger.LogWarning("All archived orders have been cleared.");
        }

        public Order GetOrder(int orderId)
        {
            var order = ActiveOrders.FirstOrDefault(o => o.Id == orderId) ?? throw new ArgumentException($"Order with ID {orderId} not found.");
            return order;
        }

        public List<Order> GetAllOrders()
        {
            return ActiveOrders;
        }

        public void SetOrderStatus(int orderId, OrderStatus status)
        {
            var order = ActiveOrders.FirstOrDefault(o => o.Id == orderId) ?? throw new ArgumentException($"Order with ID {orderId} not found.");

            order.SetStatus(status);
            if (status == OrderStatus.Cancelled || status == OrderStatus.Delivered)
            {
                ArchivedOrders.Add(new OrderArchiveSnapshot
                {
                    StatusHistory = order.StatusHistory,
                    Username = order.Username,
                    Pizzas = order.Pizzas,
                    InitialPrice = order.InitialPrice,
                    FinalPrice = order.FinalPrice,
                    PromotionName = order.PromotionName
                });
                ActiveOrders.Remove(order);
            }
            _logger.LogInfo($"Order {order.Id} status updated to {status}.");
        }

        public void AdvanceOrderStatus(int orderId)
        {
            var order = ActiveOrders.FirstOrDefault(o => o.Id == orderId) ?? throw new ArgumentException($"Order with ID {orderId} not found.");
            if (order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Cannot advance status of an order that has already been delivered or cancelled.");

            switch (order.Status)
            {
                case OrderStatus.Pending:
                    order.SetStatus(OrderStatus.InPreparation);
                    _logger.LogInfo($"Order {order.Id} is now in preparation.");
                    break;
                case OrderStatus.InPreparation:
                    order.SetStatus(OrderStatus.Ready);
                    _logger.LogInfo($"Order {order.Id} is ready for delivery.");
                    break;
                case OrderStatus.Ready:
                    order.SetStatus(OrderStatus.Delivered);
                    _logger.LogInfo($"Order {order.Id} has been delivered.");
                    ArchivedOrders.Add(new OrderArchiveSnapshot
                    {
                        StatusHistory = order.StatusHistory,
                        Username = order.Username,
                        Pizzas = order.Pizzas,
                        InitialPrice = order.InitialPrice,
                        FinalPrice = order.FinalPrice,
                        PromotionName = order.PromotionName
                    });
                    ActiveOrders.Remove(order);
                    break;
                default:
                    throw new InvalidOperationException($"Cannot advance status from {order.Status}.");
            }
        }

        public List<Order> GetActiveOrdersByStatus(OrderStatus status)
        {
            return [.. ActiveOrders.Where(o => o.Status == status)];
        }

        public List<Order> GetActiveOrdersByUserId(string username)
        {
            return [.. ActiveOrders.Where(o => o.Username == username)];
        }

        public List<OrderArchiveSnapshot> GetArchivedOrdersByUserId(string username)
        {
            return [.. ArchivedOrders.Where(o => o.Username == username)];
        }

        public bool HasUserMadeAnyOrders(string username)
        {
            return GetActiveOrdersByUserId(username).Count > 0 || GetArchivedOrdersByUserId(username).Count > 0;
        }
    }
}

