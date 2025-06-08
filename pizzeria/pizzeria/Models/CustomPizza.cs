using pizzeria.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pizzeria.Models
{
    internal class CustomPizza : PizzaBase
    {
        public CustomPizza(PizzaSize size) : base("Custom Pizza", size)
        {
        }
        public bool IsValidIngredient(Ingredient ingredient)
        {
            if (ingredient.Restriction == IngredientRestriction.OnlyLargePizza && Size != PizzaSize.Large)
                return false;

            if (ingredient.Restriction == IngredientRestriction.SingleUse && Ingredients.Contains(ingredient))
                return false;

            if (ingredient.Restriction == IngredientRestriction.NotWithMeat &&
                Ingredients.Any(i => i.Name.ToLower().Contains("meat")))
                return false;

            return true;
        }

        public void AddIngredient(Ingredient ingredient)
        {
            if (IsValidIngredient(ingredient))
            {
                Ingredients.Add(ingredient);
            }
            else
            {
                throw new System.ArgumentException($"Cannot add ingredient {ingredient.Name} due to restrictions.");
            }
        }


        public override decimal CalculatePrice()
        {
            decimal basePrice;
            switch (Size)
            {
                case PizzaSize.Small:
                    basePrice = 5.0m;
                    break;
                case PizzaSize.Medium:
                    basePrice = 7.0m;
                    break;
                case PizzaSize.Large:
                    basePrice = 10.0m;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return basePrice + Ingredients.Sum(i => i.Price);
        }

    }
}
