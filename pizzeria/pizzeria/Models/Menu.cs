using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pizzeria.Models
{
    public class Menu
    {
        private List<MenuPizza> menuPizzas;
        public Menu() 
        { 
            menuPizzas = new List<MenuPizza>();
        }
        public void AddPizza(MenuPizza pizza)
        {
            menuPizzas.Add(pizza);
        }
        public List<MenuPizza> GetAvailablePizzas()
        { 
            return menuPizzas.ToList(); 
        }
        public MenuPizza GetPizzaByName(string name, bool throwIfNotFound = false) 
        {
            var pizza = menuPizzas.FirstOrDefault(p =>
            p.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));

            if (pizza == null && throwIfNotFound)
                throw new System.ArgumentException($"Pizza {name} not found in menu.");

            return pizza;

        }
    }
}
