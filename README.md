# Order Processing System

## Overview

This project is an Order Processing System for an e-commerce application built with .NET 8. It includes models for `Customer`, `Product`, and `Order`, and it allows you to interact with the database using Entity Framework Core. The project provides REST API endpoints to manage customers and orders, including calculating order totals and custom validation rules.

## Features

### Models

- **Customer**: Represents a customer with multiple orders.
- **Product**: Represents a product with a price.
- **Order**: Represents an order containing multiple products, with a total price calculation.

### Database Setup

- **Entity Framework Core** to handle database operations and relations.
- **Migrations** to initialize and update the database schema.

### API Endpoints

- `GET /api/customers`: Retrieve all customers.
- `GET /api/customers/{id}`: Retrieve a specific customer with their orders.
- `POST /api/orders`: Create a new order specifying customer ID and product IDs.
- `GET /api/orders/{id}`: Retrieve order details including the total price.

### Business Logic

- Calculate the total price for an order.
- Validate that customers cannot place new orders while they have unfulfilled orders.

### Error Handling and Logging

- **Serilog** for structured logging.
- Meaningful error messages for validation and database errors.

### Bonus Points

- **Unit Tests** using xUnit for testing business logic and validation.
- **Docker** for containerizing the application.
- **Asynchronous Programming**: Use of `async/await` in database interactions.

## Setup Instructions

### Prerequisites

- .NET 8 SDK
- SQL Server (local or remote)

### Getting Started

1. **Clone the repository**:
    ```bash
    git clone https://github.com/[YourName]_OrderProcessingSystem.git
    cd OrderProcessingSystem
    ```

2. **Configure the database connection**:
    Update the connection string in `appsettings.json`:

    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=OrderProcessingDB;Trusted_Connection=True;"
    }
    ```

3. **Run migrations and update the database**:
    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

4. **Run the application**:
    ```bash
    dotnet run
    ```

5. **Docker** (optional):
    ```bash
    docker build -t order-processing-system .
    docker run -d -p 80:80 order-processing-system
    ```

### Endpoints Documentation

- `GET /api/customers`: Retrieve all customers.
- `GET /api/customers/{id}`: Retrieve details for a specific customer, including their orders.
- `POST /api/orders`: Create a new order for a customer, specifying the customer ID and a list of product IDs.
- `GET /api/orders/{id}`: Retrieve details for a specific order, including the total price.

## Additional Notes

- Ensure the application uses meaningful logging and error handling.
- Follow best practices for clean and maintainable code.
- Write unit tests to validate business logic and error handling.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

Feel free to refine or expand upon this README to suit your needs!

## Step 13: Unit Tests with xUnit

It's important to write unit tests to ensure that the business logic is functioning as expected.

### Tests/OrderServiceTests.cs

```csharp
namespace OrderProcessingSystem.Tests
{
    public class OrderServiceTests
    {
        private readonly IOrderService _orderService;
        private readonly ApplicationDbContext _dbContext;

        public OrderServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _dbContext = new ApplicationDbContext(dbContextOptions);
            _orderService = new OrderService(_dbContext);
        }

        [Fact]
        public async Task CreateOrder_Success()
        {
            // Arrange
            var customer = new Customer { Name = "John Doe" };
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();
    
            var product = new Product { Name = "Product1", Price = 100 };
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            var productIds = new List<int> { product.ProductId };

            // Act
            var order = await _orderService.CreateOrderAsync(customer.CustomerId, productIds);

            // Assert
            Assert.NotNull(order);
            Assert.Equal(product.Price, order.TotalPrice);
        }

        [Fact]
        public async Task GetOrderById_ShouldReturnOrder()
        {
            // Arrange
            var customer = new Customer { Name = "Jane Doe" };
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();

            var product = new Product { Name = "Product2", Price = 200 };
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            var productIds = new List<int> { product.ProductId };

            var order = await _orderService.CreateOrderAsync(customer.CustomerId, productIds);

            // Act
            var fetchedOrder = await _orderService.GetOrderByIdAsync(order.OrderId);

            // Assert
            Assert.NotNull(fetchedOrder);
            Assert.Equal(order.OrderId, fetchedOrder.OrderId);
        }
    }
}
