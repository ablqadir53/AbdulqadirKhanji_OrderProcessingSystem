using OrderProcessingSystem.Models;

namespace OrderProcessingSystem.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int customerId, List<int> productIds);
        Task<Order> GetOrderByIdAsync(int orderId);
    }
}
