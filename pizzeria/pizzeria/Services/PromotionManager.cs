using pizzeria.Interfaces;
using pizzeria.Models;
namespace pizzeria.Services
{
    public class PromotionManager : IPromotionManager
    {
        public required List<IPromotion> Promotions { get; init; } = new List<IPromotion>();

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
