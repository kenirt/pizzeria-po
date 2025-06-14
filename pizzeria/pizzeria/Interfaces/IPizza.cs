using pizzeria.Enums;

namespace pizzeria.Interfaces
{
    public interface IPizza
    {
        string Name { get; }
        PizzaSize Size { get; }

        decimal CalculatePrice();
    }
}

