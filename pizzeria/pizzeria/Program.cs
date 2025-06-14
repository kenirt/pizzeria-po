using pizzeria.Services;
using pizzeria.Interfaces;
using pizzeria.UI;
using pizzeria.Models;
using pizzeria.Enums;

namespace pizzeria;
public class Program
{
    public static void Main()
    {
        ILogger logger = new FileLogger();
        IPromotionManager promotionManager = new PromotionManager();
        IOrderQueue orderQueue = new OrderQueue(promotionManager, logger);
        IUserManager userManager = new UserManager(logger);
        SessionService sessionService = new SessionService(logger, userManager);
        
        var menu = new Menu
        {
            MenuItems = new List<MenuItem>
            {
                new() { Name = "Margherita", Price = 8.99m},
                new() { Name = "Pepperoni", Price = 10.99m},
                new() { Name = "Vegetarian", Price = 9.49m},
                new() { Name = "BBQ Chicken", Price = 11.49m},
                new() { Name = "Hawaiian", Price = 10.49m},
            },
            Ingredients = new List<Ingredient>
            {
                new() { Name = "Tomato Sauce", Price = 0.50m, Restriction = IngredientRestriction.None },
                new() { Name = "Mozzarella Cheese", Price = 1.00m, Restriction = IngredientRestriction.None },
                new() { Name = "Pepperoni", Price = 1.50m, Restriction = IngredientRestriction.None }
            }
        };

        MainUI mainUI = new MainUI(logger, userManager, orderQueue, sessionService, menu);

        try
        {
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

