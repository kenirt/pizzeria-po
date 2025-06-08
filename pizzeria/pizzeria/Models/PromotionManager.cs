using pizzeria.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pizzeria.Models
{
    public class PromotionManager
    {
        public void ApplyPromotions(List<Order> orders)
        {
            if (orders.Count >= 3)
            {
                var pendingOrders = orders.Where(o => o.Status == OrderStatus.Pending).ToList();
                if (pendingOrders.Count >= 3)
                {
                    var cheapestOrder = pendingOrders.OrderBy(o => o.FinalPrice).First();
                    cheapestOrder.FinalPrice = 0m;
                    System.Console.WriteLine($"Promotion applied, Pizza {cheapestOrder.Id} is free!");
                }

            }

        }

    }
}
