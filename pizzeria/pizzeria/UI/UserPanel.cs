using pizzeria.Interfaces;
using pizzeria.Models;
using pizzeria.Enums;

namespace pizzeria.UI
{
    public class UserPanel
    {
        private readonly IOrderQueue _orderQueue;
        private readonly string _username;
        private readonly Menu _menu;
        private List<IPizza> _pizzas = new List<IPizza>();

        public UserPanel(IOrderQueue orderQueue, string name, Menu menu)
        {
            _orderQueue = orderQueue ?? throw new ArgumentNullException(nameof(orderQueue));
            _username = name ?? throw new ArgumentNullException(nameof(name));
            _menu = menu;
        }

        private void ViewOrders()
        {
            var listOrders = new ListOrders(_orderQueue);
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Your Active Orders:");
                var activeOrders = _orderQueue.GetActiveOrdersByUserId(_username)
                    .Where(o => o.Status == OrderStatus.Pending)
                    .ToList();

                if (activeOrders.Count == 0)
                {
                    Console.WriteLine("You have no pending orders.");
                }
                else
                {
                    for (int i = 0; i < activeOrders.Count; i++)
                    {
                        var order = activeOrders[i];
                        Console.WriteLine($"{i + 1}. Placed: {order.StatusHistory.First().Timestamp:g}, Price: {order.FinalPrice:C}, Status: {order.Status}");
                    }
                    Console.WriteLine();
                    Console.Write("Enter the number of the order to cancel, or press Enter to skip: ");
                    var input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        if (int.TryParse(input, out int idx) && idx > 0 && idx <= activeOrders.Count)
                        {
                            var orderToCancel = activeOrders[idx - 1];
                            try
                            {
                                _orderQueue.CancelOrder(orderToCancel.Id, false, _username);
                                Console.WriteLine("Order cancelled successfully.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Could not cancel order: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid selection. No order cancelled.");
                        }
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        continue; // Refresh the list after cancellation attempt
                    }
                }

                Console.WriteLine();
                Console.Write("Would you like to see your order history? (y/n): ");
                var historyInput = Console.ReadLine();
                if (historyInput?.Trim().ToLower() == "y")
                {
                    Console.WriteLine();
                    Console.WriteLine("Your Order History:");
                    listOrders.ShowUserArchivedOrders(_username);
                    Console.WriteLine();
                    Console.WriteLine("Press any key to return to the previous menu.");
                    Console.ReadKey();
                }
                break;
            }
        }

        private void AddPizzaToOrder()
        {
            Console.Clear();
            Console.WriteLine("Adding a pizza to your order...");
            Console.WriteLine("1. Select from the Menu");
            Console.WriteLine("2. Create a Custom Pizza");
            Console.WriteLine("Press any other key to return to the main menu.");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.Clear();
                    ViewMenu();
                    Console.Write("Enter the number of the pizza you want to add: ");
                    var pizzaChoice = Console.ReadLine();
                    if (int.TryParse(pizzaChoice, out int pizzaIndex) && pizzaIndex > 0 && pizzaIndex <= _menu.MenuItems.Count)
                    {
                        var selectedPizza = _menu.MenuItems[pizzaIndex - 1];
                        Console.WriteLine($"Select size (S/M/L): ");
                        var sizeChoice = Console.ReadLine()?.ToUpper();
                        switch (sizeChoice)
                        {
                            case "S":
                                _pizzas.Add(new MenuPizza(selectedPizza.Name, PizzaSize.Small, selectedPizza.Price));
                                break;
                            case "M":
                                _pizzas.Add(new MenuPizza(selectedPizza.Name, PizzaSize.Medium, selectedPizza.Price));
                                break;
                            case "L":
                                _pizzas.Add(new MenuPizza(selectedPizza.Name, PizzaSize.Large, selectedPizza.Price));
                                break;
                            default:
                                Console.WriteLine("Invalid size selection. Press any key to try again.");
                                Console.ReadKey();
                                return;
                        }
                        Console.WriteLine($"Added {selectedPizza.Name} to your order.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection. Press any key to try again.");
                        Console.ReadKey();
                        return;
                    }

                    break;
                case "2":
                    var ingredients = new List<Ingredient>();
                    var ingredientCounts = new Dictionary<int, int>(); // ingredient index, value: count

                    while (true)
                    {
                        Console.Clear();
                        ViewIngredients();

                        // Display current ingredient setup
                        Console.WriteLine("Current pizza ingredients:");
                        if (ingredientCounts.Count == 0)
                        {
                            Console.WriteLine("  No ingredients added yet.");
                        }
                        else
                        {
                            foreach (var kvp in ingredientCounts)
                            {
                                var ingredient = _menu.Ingredients[kvp.Key - 1];
                                Console.WriteLine($"  {ingredient.Name} x{kvp.Value}");
                            }
                        }
                        Console.WriteLine();

                        Console.WriteLine("Type 'done' when you are finished adding ingredients.");
                        Console.Write("Enter the number of the ingredient you want to add: ");
                        var ingredientChoice = Console.ReadLine();
                        if (ingredientChoice?.ToLower() == "done")
                        {
                            break;
                        }

                        if (int.TryParse(ingredientChoice, out int ingredientIndex) && ingredientIndex > 0 && ingredientIndex <= _menu.Ingredients.Count)
                        {
                            var selectedIngredient = _menu.Ingredients[ingredientIndex - 1];

                            // Restriction: MaxOne
                            if (selectedIngredient.Restriction == IngredientRestriction.MaxOne && ingredientCounts.ContainsKey(ingredientIndex))
                            {
                                Console.WriteLine($"{selectedIngredient.Name} can only be added once.");
                                Console.WriteLine("Press any key to continue...");
                                Console.ReadKey();
                                continue;
                            }

                            // Restriction: MaxTwo
                            ingredientCounts.TryGetValue(ingredientIndex, out int count);
                            if (selectedIngredient.Restriction == IngredientRestriction.MaxTwo && count >= 2)
                            {
                                Console.WriteLine($"You can only add up to 2 of {selectedIngredient.Name}.");
                                Console.WriteLine("Press any key to continue...");
                                Console.ReadKey();
                                continue;
                            }

                            // Max 3 of any ingredient
                            if (count >= 3)
                            {
                                Console.WriteLine($"You can only add up to 3 of any igredient.");
                                Console.WriteLine("Press any key to continue...");
                                Console.ReadKey();
                                continue;
                            }

                            // Add ingredient
                            ingredients.Add(selectedIngredient);
                            ingredientCounts[ingredientIndex] = count + 1;
                        }
                    }

                    if (ingredients.Count == 0)
                    {
                        Console.WriteLine("No ingredients added. Returning to the main menu.");
                        return;
                    }
                    Console.WriteLine("Select pizza size (S/M/L): ");
                    var sizeInput = Console.ReadLine()?.ToUpper();
                    PizzaSize size;
                    switch (sizeInput)
                    {
                        case "S":
                            size = PizzaSize.Small;
                            break;
                        case "M":
                            size = PizzaSize.Medium;
                            break;
                        case "L":
                            size = PizzaSize.Large;
                            break;
                        default:
                            Console.WriteLine("Invalid size selection. Press any key to try again.");
                            Console.ReadKey();
                            return;
                    }
                    var customPizza = new CustomPizza("Custom Pizza", size, ingredients);
                    _pizzas.Add(customPizza);
                    Console.WriteLine($"Added custom pizza with {ingredients.Count} ingredients to your order.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                default:
                    return;
            }
        }
        
        private void MakeOrder()
        {
            Console.Clear();
            Console.WriteLine("Creating a new order...");

            while (true)
            {
                AddPizzaToOrder();
                Console.WriteLine("Do you want to add another pizza? (Y/N)");
                var continueChoice = Console.ReadLine()?.ToUpper();
                if (continueChoice != "Y")
                {
                    break;
                }
            }

            if (_pizzas.Count == 0)
            {
                Console.WriteLine("No _pizzas added to the order. Returning to the main menu.");
                return;
            }

            
            int id = _orderQueue.PlaceOrder(_username, _pizzas);
            Console.WriteLine($"Your order has been placed successfully! Order ID: {id}");
        }

        private void ViewMenu()
        {
            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine("{0,-3} {1,-20} {2,10} {3,10} {4,10}", "#", "Pizza", "Small", "Medium", "Large");
            Console.WriteLine("-------------------------------------------------------------------------");

            for (int i = 0; i < _menu.MenuItems.Count; i++)
            {
                var item = _menu.MenuItems[i];
                decimal small = Math.Round(item.Price * 0.8m, 2);
                decimal medium = Math.Round(item.Price * 1.0m, 2);
                decimal large = Math.Round(item.Price * 1.2m, 2);

                Console.WriteLine("{0,-3} {1,-20} {2,10:C} {3,10:C} {4,10:C}", i + 1, item.Name, small, medium, large);
            }

            Console.WriteLine("-------------------------------------------------------------------------");
        }

        private void ViewIngredients()
        {
            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine("{0,-3} {1,-20} {2,10}", "#", "Ingredient", "Price");
            Console.WriteLine("-------------------------------------------------------------------------");

            for (int i = 0; i < _menu.Ingredients.Count; i++)
            {
                var ingredient = _menu.Ingredients[i];
                Console.WriteLine("{0,-3} {1,-20} {2,10:C}", i + 1, ingredient.Name, ingredient.Price);
            }

            Console.WriteLine("-------------------------------------------------------------------------");
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. View Your Orders");
                Console.WriteLine("2. Place an Order");
                Console.WriteLine("Press any other key to exit.");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewOrders();
                        break;
                    case "2":
                        MakeOrder();
                        break;
                    default:
                        Console.WriteLine("Exiting User Panel. Thank you for using our service!");
                        return;
                }
            }
        }
    }
}