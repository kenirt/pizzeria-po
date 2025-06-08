using pizzeria.Enums;
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
        public decimal FinalPrice;
    }
}
