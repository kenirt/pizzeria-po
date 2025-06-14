using pizzeria.Models;
using pizzeria.Interfaces;
using pizzeria.Enums;
using System.Text.Json;

namespace pizzeria.Models
{
    public class Menu
    {
        public List<MenuItem> MenuItems { get; init; } = new List<MenuItem>();
        public List<Ingredient> Ingredients { get; init; } = new List<Ingredient>();
    }

    public class MenuItem
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public required string Name { get; init; }
        public required decimal Price { get; init; }
    }

    public class Ingredient
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public required string Name { get; init; }
        public required decimal Price { get; init; }
        public required IngredientRestriction Restriction { get; init; }
    }
}