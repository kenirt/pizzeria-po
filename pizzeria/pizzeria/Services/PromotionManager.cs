using pizzeria.Interfaces;
using pizzeria.Models;
namespace pizzeria.Services
{
    public class PromotionManager : IPromotionManager
    {
        public List<IPromotion> Promotions { get; } = new()
        {
            new BuyXGetYFreePromotion
            {
                Name = "Buy 2 Get 1 Free",
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(30),
                X = 2,
                Y = 1
            },
            new FirstOrderPromotion
            {
                Name = "First Order Discount",
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(30),
                DiscountPercentage = 10
            },
            new PercentagePromotion
            {
                Name = "Summer 10% Off",
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(30),
                DiscountPercentage = 10
            }
        };

        public IEnumerable<IPromotion> GetActivePromotions()
        {
            return Promotions.Where(p => p.IsActive());
        }

        public (string? Name, decimal Discount) GetBestPromotion(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }

            var applicablePromotions = Promotions
                .Where(p => p.IsActive() && p.IsApplicable(order))
                .ToList();

            if (applicablePromotions.Count == 0)
            {
                return (null, 0);
            }

            var bestPromotion = applicablePromotions
                .OrderByDescending(p => p.CalculateDiscount(order))
                .First();

            return (bestPromotion.Name, bestPromotion.CalculateDiscount(order));
        }
    }
}
