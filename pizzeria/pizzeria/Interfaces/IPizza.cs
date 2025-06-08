using pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pizzeria.Interfaces
{
    public interface IPizza
    {
        string Name;
        PizzaSize Size;
        List<Ingredient> Ingredients;
        decimal CalculatePrice();
    }
}
