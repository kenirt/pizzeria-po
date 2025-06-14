using pizzeria.Models;

namespace pizzeria.Interfaces
{
    public interface IPromotionManager
    {
        List<IPromotion> Promotions { get; }
        IEnumerable<IPromotion> GetActivePromotions();
        (string? Name, decimal Discount) GetBestPromotion(Order order);
    }
}
