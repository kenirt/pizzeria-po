using pizzeria.Interfaces;

namespace pizzeria.Models
{
    public abstract class PromotionBase : IPromotion
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }

        public bool IsActive() =>
            StartDate <= DateTime.Now && DateTime.Now <= EndDate;
        public abstract bool IsApplicable(Order order);
        public abstract decimal CalculateDiscount(Order order);
    }

    public class PercentagePromotion : PromotionBase
    {
        public decimal DiscountPercentage { get; init; }

        public override bool IsApplicable(Order order)
        {
            return true;
        }
        public override decimal CalculateDiscount(Order order)
        {
            return Math.Round(order.InitialPrice * (DiscountPercentage / 100), 2);
        }
    }

    public class FirstOrderPromotion : PromotionBase
    {
        public decimal DiscountPercentage { get; init; }

        public override bool IsApplicable(Order order)
        {
            return order.IsFirstOrder;
        }

        public override decimal CalculateDiscount(Order order)
        {
            // Calculate the discount based on the total price of the order
            return Math.Round(order.InitialPrice * (DiscountPercentage / 100),2);
        }
    }

    public class BuyXGetYFreePromotion : PromotionBase
    {
        public int X { get; init; }
        public int Y { get; init; }

        public override bool IsApplicable(Order order)
        {
            return order.Pizzas.Count >= X + Y;
        }

        public override decimal CalculateDiscount(Order order)
        {
            // Calculate the discount based on the cheapest pizza in the order
            var pizzasToGetFree = order.Pizzas.OrderBy(p => p.Price).Take(Y).ToList();
            return Math.Round(pizzasToGetFree.Sum(p => p.Price),2);
        }
    }

    public class MinOrderValuePromotion : PromotionBase
    {
        public decimal MinOrderValue { get; init; }
        public decimal DiscountAmount { get; init; }

        public override bool IsApplicable(Order order)
        {
            return order.InitialPrice >= MinOrderValue;
        }

        public override decimal CalculateDiscount(Order order)
        {
            return Math.Round(DiscountAmount,2);
        }
    }
}


