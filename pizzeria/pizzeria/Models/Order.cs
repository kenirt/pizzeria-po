using pizzeria.Enums;

namespace pizzeria.Models
{
    public class Order
    {
        public required int Id { get; init; }
        public OrderStatus Status { get; private set; }
        public List<OrderStatusHistoryEntry> StatusHistory { get; private set; }
        public required string Username { get; init; }
        public required List<OrderPizzaSnapshot> Pizzas { get; init; }
        public required decimal InitialPrice { get; init; }
        public decimal FinalPrice { get; private set; }
        public string? PromotionName { get; private set; }
        public required bool IsFirstOrder { get; init; }

        public Order()
        {
            Status = OrderStatus.Pending;
            StatusHistory =
            [
                new OrderStatusHistoryEntry
                {
                    Status = Status,
                    Timestamp = DateTime.UtcNow
                }
            ];
            FinalPrice = InitialPrice;
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

