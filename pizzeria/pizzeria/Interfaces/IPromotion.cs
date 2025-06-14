using pizzeria.Models;

namespace pizzeria.Interfaces
{
    public interface IPromotion
    {
        public Guid Id { get; }
        string Name { get; }
        DateTime StartDate { get; }
        DateTime EndDate { get; }
        bool IsActive();
        bool IsApplicable(Order order);
        decimal CalculateDiscount(Order order);
    }
}
