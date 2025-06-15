using pizzeria.Enums;
using pizzeria.Models;

namespace pizzeria.Interfaces
{
    public interface IOrderQueue
    {
        List<Order> ActiveOrders { get; }
        List<OrderArchiveSnapshot> ArchivedOrders { get; }

        void Initialize();
        void Shutdown();
        int PlaceOrder(string username, List<IPizza> pizzas);
        void CancelOrder(int orderId, bool employee = false, string username = "");
        void CancelAllOrders();
        void ClearOrderHistory();
        Order GetOrder(int orderId);
        List<Order> GetAllOrders();
        void SetOrderStatus(int orderId, OrderStatus status);
        void AdvanceOrderStatus(int orderId);
        List<Order> GetActiveOrdersByStatus(OrderStatus status);
        List<Order> GetActiveOrdersByUserId(string username);
        List<OrderArchiveSnapshot> GetArchivedOrdersByUserId(string username);
    }
}
