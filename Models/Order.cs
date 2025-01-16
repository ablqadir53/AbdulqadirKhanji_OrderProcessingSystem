namespace OrderProcessingSystem.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
        public bool IsFulfilled { get; set; }
        public decimal TotalPrice
        {
            get
            {
                return OrderProducts?.Sum(op => op.Product.Price) ?? 0m;
            }
        }
    }

    public class OrderProduct
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
