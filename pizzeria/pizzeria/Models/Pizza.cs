using pizzeria.Enums;
using pizzeria.Interfaces;

namespace pizzeria.Models
{
    public abstract class PizzaBase(string name, PizzaSize size) : IPizza
    {
        public string Name { get; protected set; } = name;
        public PizzaSize Size { get; protected set; } = size;

        public abstract decimal CalculatePrice();
    }

    public class MenuPizza(string name, PizzaSize size, decimal basePrice) : PizzaBase(name, size)
    {
        private readonly decimal BasePrice = basePrice;

        public override decimal CalculatePrice()
        {
            var sizeMultiplier = Size switch
            {
                PizzaSize.Small => 0.8m,
                PizzaSize.Medium => 1.0m,
                PizzaSize.Large => 1.2m,
                _ => 1.0m
            };

            return Math.Round(BasePrice * sizeMultiplier, 2);
        }
    }

    public class CustomPizza(string name, PizzaSize size, List<Ingredient> ingredients) : PizzaBase(name, size)
    {
        public List<Ingredient> Ingredients { get; private set; } = ingredients;

        public override decimal CalculatePrice()
        {
            var basePrice = Size switch
            {
                PizzaSize.Small => 5.0m,
                PizzaSize.Medium => 7.0m,
                PizzaSize.Large => 10.0m,
                _ => throw new ArgumentOutOfRangeException(),
            };
            return basePrice + Ingredients.Sum(i => i.Price);
        }

    }
}

