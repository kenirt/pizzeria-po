using pizzeria.Interfaces;
using pizzeria.Models;

namespace pizzeria.UI
{
    public class ListOrders
    {
        private readonly IOrderQueue _orderQueue;

        public ListOrders(IOrderQueue orderQueue)
        {
            _orderQueue = orderQueue;
        }

        public void ShowAllActiveOrders()
        {
            var orders = _orderQueue.ActiveOrders;
            Console.WriteLine("Active Orders:");
            foreach (var order in orders)
            {
                PrintOrder(order);
            }
        }

        public void ShowAllArchivedOrders()
        {
            var orders = _orderQueue.ArchivedOrders;
            Console.WriteLine("Order History:");
            foreach (var order in orders)
            {
                PrintArchivedOrder(order);
            }
        }

        public void ShowUserActiveOrders(string username)
        {
            var orders = _orderQueue.GetActiveOrdersByUserId(username);
            if (orders.Count == 0)
            {
                Console.WriteLine($"No active orders found.");
                return;
            }

            Console.WriteLine($"Active Orders:");
            foreach (var order in orders)
            {
                PrintOrder(order);
            }
        }
        public void ShowUserArchivedOrders(string username)
        {
            var orders = _orderQueue.GetArchivedOrdersByUserId(username);
            if (orders.Count == 0)
            {
                Console.WriteLine($"No archived orders found.");
                return;
            }

            foreach (var order in orders)
            {
                PrintArchivedOrder(order);
            }
        }

        private void PrintOrder(Order order)
        {
            Console.WriteLine($"Order ID: {order.Id}, Status: {order.Status}, Price: {order.FinalPrice:C}");
        }

        private void PrintArchivedOrder(OrderArchiveSnapshot order)
        {
            Console.WriteLine($"Order by {order.Username}, Final Price: {order.FinalPrice:C}, Promotion: {order.PromotionName}");
        }
    }
}