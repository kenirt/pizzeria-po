using pizzeria.Services;
using pizzeria.Interfaces;
using pizzeria.UI;
using pizzeria.Models;
using pizzeria.Enums;
using pizzeria.Utils;

namespace pizzeria;
public class Program
{
    public static void Main()
    {
        ILogger logger = new FileLogger();
        IPromotionManager promotionManager = new PromotionManager
        {
            Promotions = new List<IPromotion>
            {
                new BuyXGetYFreePromotion
                {
                    Name = "Buy 2 Get 1 Free",
                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now.AddDays(30),
                    X = 2,
                    Y = 1
                },
                new FirstOrderPromotion
                {
                    Name = "First Order Discount",
                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now.AddDays(30),
                    DiscountPercentage = 10
                },
                new PercentagePromotion
                {
                    Name = "Summer 10% Off",
                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now.AddDays(30),
                    DiscountPercentage = 10
                }
            }
        };

        IOrderQueue orderQueue = new OrderQueue(promotionManager, logger);
        IUserManager userManager = new UserManager(logger);
        SessionService sessionService = new SessionService(logger, userManager);


        var ingredients = new List<Ingredient>
            {
                new() { Name = "Tomato Sauce", Price = 0.50m, Restriction = IngredientRestriction.None, Type = IngredientType.Sauce },
                new() { Name = "Marinara Sauce", Price = 0.50m, Restriction = IngredientRestriction.None, Type = IngredientType.Sauce },
                new() { Name = "Barbecue Sauce", Price = 0.50m, Restriction = IngredientRestriction.None, Type = IngredientType.Sauce },
                new() { Name = "Pesto Sauce", Price = 0.50m, Restriction = IngredientRestriction.None, Type = IngredientType.Sauce },
                new() { Name = "Mozzarella Cheese", Price = 1.00m, Restriction = IngredientRestriction.None, Type = IngredientType.Cheese },
                new() { Name = "Pepperoni", Price = 1.50m, Restriction = IngredientRestriction.None, Type = IngredientType.Meat },
                new() { Name = "Ham", Price = 1.50m, Restriction = IngredientRestriction.None, Type = IngredientType.Meat },
                new() { Name = "Pineapple", Price = 1.50m, Restriction = IngredientRestriction.None, Type = IngredientType.Vegetable },
                new() { Name = "chicken", Price = 1.50m, Restriction = IngredientRestriction.None, Type = IngredientType.Meat }
           };
        var menu = new Menu
        {
            MenuItems = new List<MenuItem>
            {
                new() { Name = "Margherita", Price = 8.99m, Ingredients = new List<Ingredient> { ingredients[0], ingredients[4], ingredients[5] }},
                new() { Name = "Pepperoni", Price = 10.99m, Ingredients = new List<Ingredient> { ingredients[0], ingredients[6], ingredients[7] }},
                new() { Name = "Vegetarian", Price = 9.49m, Ingredients = new List<Ingredient> { ingredients[0], ingredients[7] }},
                new() { Name = "BBQ Chicken", Price = 11.49m, Ingredients = new List<Ingredient> { ingredients[2], ingredients[4], ingredients[8]  }},
                new() { Name = "Hawaiian", Price = 10.49m, Ingredients = new List < Ingredient > { ingredients[0], ingredients[7] }},
            },
            Ingredients = ingredients
        };

        MainUI mainUI = new MainUI(logger, userManager, orderQueue, sessionService, menu);

        try
        {
            Console.WriteLine(HashPassword.Hash("employee"));
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            orderQueue.Initialize();
            userManager.Initialize();
            mainUI.Run();

            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("Shutting down gracefully...");
                orderQueue.Shutdown();
                userManager.Shutdown();
                logger.LogInfo("Application shutdown initiated.");

                e.Cancel = true;
                Environment.Exit(0);
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while running the application.");
            logger.LogError($"An error occurred: {ex.Message}");
        }
        finally
        {
            // Ensure all services are shut down properly
            logger.LogInfo("Shutting down services...");
            userManager.Shutdown();
            orderQueue.Shutdown();
        }
    }
}

