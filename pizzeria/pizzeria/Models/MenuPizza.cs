using pizzeria.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pizzeria.Models
{
    public class MenuPizza : PizzaBase
    {
        private decimal BasePrice;

        public MenuPizza(string name, PizzaSize size, decimal basePrice) : base(name, size)
        {
            BasePrice = basePrice;
        }
        public override decimal CalculatePrice()
        {
            decimal sizeMultiplier;

            switch (Size)
            {
                case PizzaSize.Small:
                    sizeMultiplier = 0.8m;
                    break;
                case PizzaSize.Medium:
                    sizeMultiplier = 1.0m;
                    break;
                case PizzaSize.Large:
                    sizeMultiplier = 1.2m;
                    break;
                default:
                    sizeMultiplier = 1.0m;
                    break;
            }

            return BasePrice * sizeMultiplier;
        }
    }
}
