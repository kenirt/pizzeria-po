using pizzeria.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pizzeria.Models
{
    public class Ingredient
    {
        public string Name;
        public decimal Price;
        public IngredientRestriction Restriction;

        public Ingredient(string name, decimal price, IngredientRestriction restriction = IngredientRestriction.None)
        {
            Name = name;
            Price = price;
            Restriction = restriction;

        }
    }
    
}
