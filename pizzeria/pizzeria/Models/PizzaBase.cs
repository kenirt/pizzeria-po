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
        public string Name { get; protected set; }
        public PizzaSize Size { get; protected set; }
        public List<Ingredient> Ingredients { get; protected set; }


        protected PizzaBase(string name, PizzaSize size) 
        {  
            Name = name;
            Size = size;
            Ingredients = new List<Ingredient>();

        }

        public abstract decimal CalculatePrice();
    }
}
