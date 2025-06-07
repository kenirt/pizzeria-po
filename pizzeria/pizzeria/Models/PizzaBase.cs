using pizzeria.Enums;
using pizzeria.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pizzeria.Models
{
    public abstract class PizzaBase: IPizza
    {
        public string Name;
        public PizzaSize Size;
        public List<Ingredient> Ingredients;

        protected PizzaBase(string name, string size) 
        {  
            Name = name;
            Size = size;
            Ingredients = new List<Ingredient>();

        }

        public abstract decimal CalculatePrice();
    }
}
