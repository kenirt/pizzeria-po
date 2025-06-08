using pizzeria.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pizzeria.Models
{
    public class OrderQueue
    {
        private readonly List<Order> _orders;
        private int _nextId;

        public OrderQueue()
        {
            _orders = new List<Order>();
            _nextId = 1;
        }

        public void AddOrder(IPizza pizza)
        {
            var order = new Order(_nextId++, pizza);
            _orders.Add(order);
        }

        public void CancelOrder(int orderId)
        {
            var order = _orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
                throw new ArgumentException($"Order with ID {orderId} not found.");

            order.Cancel();
        }

        public void UpdateStatus(int orderId)
        {
            var order = _orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
                throw new ArgumentException($"Order with ID {orderId} not found.");

            order.AdvanceStatus();
        }

        public void ListOrders()
        {
            foreach (var order in _orders)
            {
                Console.WriteLine($"Order #{order.Id}: {order.Pizza.Name} ({order.Pizza.Size}), Status: {order.Status}, Price: ${order.FinalPrice:F2}, Placed: {order.TimePlaced}");
            }
        }

        public List<Order> GetOrders()
        {
            return _orders.ToList();
        }
    }

}
