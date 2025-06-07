using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pizzeria.Interfaces
{
    internal interface IPizza
    {
        string Name;
        PizzaSize Size;
        List<Ingredient> Ingredients;
        decimal CalculatePrice();
    }
}
