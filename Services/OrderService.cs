using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.Data;
using OrderProcessingSystem.Models;

namespace OrderProcessingSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(int customerId, List<int> productIds)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) throw new Exception("Customer not found");

            var previousUnfulfilledOrder = await _context.Orders
                .Where(o => o.CustomerId == customerId && !o.IsFulfilled)
                .FirstOrDefaultAsync();
            if (previousUnfulfilledOrder != null)
            {
                throw new Exception("Customer has unfulfilled orders");
            }

            var products = await _context.Products.Where(p => productIds.Contains(p.ProductId)).ToListAsync();
            var order = new Order
            {
                CustomerId = customerId,
                IsFulfilled = false,
                OrderProducts = products.Select(p => new OrderProduct { ProductId = p.ProductId }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            return order;
        }
    }
}
