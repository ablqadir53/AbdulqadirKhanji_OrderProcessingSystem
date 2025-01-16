using OrderProcessingSystem.Models;

namespace OrderProcessingSystem.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Customers.Any())
            {
                return; // DB has been seeded
            }

            var customers = new[]
            {
                new Customer { Name = "John Doe" },
                new Customer { Name = "Jane Smith" },
            };

            var products = new[]
            {
                new Product { Name = "Product 1", Price = 100 },
                new Product { Name = "Product 2", Price = 200 },
            };

            var orders = new[]
            {
                new Order { CustomerId = 1, IsFulfilled = false },
                new Order { CustomerId = 2, IsFulfilled = false },
            };

            var orderProducts = new[]
            {
                new OrderProduct { OrderId = 1, ProductId = 1 },
                new OrderProduct { OrderId = 2, ProductId = 2 },
            };

            context.Customers.AddRange(customers);
            context.Products.AddRange(products);
            context.Orders.AddRange(orders);
            context.OrderProducts.AddRange(orderProducts);

            context.SaveChanges();
        }
    }
}
