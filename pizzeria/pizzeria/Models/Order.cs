using System.Text.Json.Serialization;
using pizzeria.Enums;

namespace pizzeria.Models
{
    public class Order
    {
        public int Id { get; private set; }
        public OrderStatus Status { get; private set; }
        public List<OrderStatusHistoryEntry> StatusHistory { get; private set; }
        public string Username { get; private set; }
        public List<OrderPizzaSnapshot> Pizzas { get; private set; }
        public decimal InitialPrice { get; private set; }
        public decimal FinalPrice { get; private set; }
        public string? PromotionName { get; private set; }
        public bool IsFirstOrder { get; private set; }

        [JsonConstructor]
        public Order(int id, OrderStatus status, List<OrderStatusHistoryEntry> statusHistory, string username,
                     List<OrderPizzaSnapshot> pizzas, decimal initialPrice, bool isFirstOrder, decimal finalPrice = 0,
                     string? promotionName = null)
        {
            Id = id;
            Status = status;
            StatusHistory = statusHistory ?? new List<OrderStatusHistoryEntry>();
            Username = username;
            Pizzas = pizzas ?? new List<OrderPizzaSnapshot>();
            InitialPrice = initialPrice;
            FinalPrice = finalPrice;
            PromotionName = promotionName;
            IsFirstOrder = isFirstOrder;
        }

        public void SetStatus(OrderStatus status)
        {
            Status = status;
            StatusHistory.Add(new OrderStatusHistoryEntry
            {
                Status = status,
                Timestamp = DateTime.UtcNow
            });
        }

        public void SetPromotionName(string promotionName)
        {
            if (string.IsNullOrWhiteSpace(promotionName))
                throw new ArgumentException("Promotion name cannot be null or empty.", nameof(promotionName));
            PromotionName = promotionName;
        }

        public void SetFinalPrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
            FinalPrice = price;
        }
    }

    public class OrderPizzaSnapshot
    {
        public required string Name { get; init; }
        public required string Size { get; init; }
        public required decimal Price { get; init; }
        public List<string>? Ingredients { get; init; }

        public OrderPizzaSnapshot() { }
    }

    public class OrderStatusHistoryEntry
    {
        public required OrderStatus Status { get; init; }
        public required DateTime Timestamp { get; init; }

        public OrderStatusHistoryEntry() { }
    }

    public class OrderArchiveSnapshot
    {
        public required List<OrderStatusHistoryEntry> StatusHistory { get; init; }
        public required string Username { get; init; }
        public required List<OrderPizzaSnapshot> Pizzas { get; init; }
        public required decimal InitialPrice { get; init; }
        public required decimal FinalPrice { get; init; }
        public required string? PromotionName { get; init; }

        public OrderArchiveSnapshot() { }
    }
}

