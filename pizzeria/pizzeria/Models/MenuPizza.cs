using pizzeria.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pizzeria.Models
{
    internal class MenuPizza : PizzaBase
    {
        private decimal BasePrice;

        public MenuPizza(string name, PizzaSize size, decimal basePrice) : base(name, size)
        {
            BasePrice = basePrice;
        }

    }
}
