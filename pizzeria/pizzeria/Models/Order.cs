using pizzeria.Enums;
using pizzeria.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pizzeria.Models
{
    public class Order
    {
        public int Id;
        public DateTime TimePlaced;
        public OrderStatus Status;
        public IPizza Pizza;
        public decimal FinalPrice;
        public Order(int id, IPizza pizza)
        {
            Id = id;
            Pizza = pizza;
            TimePlaced = DateTime.Now;
            Status = OrderStatus.Pending;
            FinalPrice = pizza.CalculatePrice();
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Pending)
            {
                Status = OrderStatus.Cancelled;
            }
            else
            {
                Console.WriteLine("Cannot cancel order");
            }
        }

        public void AdvanceStatus()
        {
            if (Status == OrderStatus.Cancelled)
            {
                throw new InvalidOperationException();
            }
            switch (Status)
            {
                case OrderStatus.Pending:
                    Status = OrderStatus.InPreparation;
                    break;
                case OrderStatus.InPreparation:
                    Status = OrderStatus.Ready;
                    break;
                case OrderStatus.Ready:
                    Status = OrderStatus.Ready;
                    break;
                default:
                    break;

            }
        }
    }
}
