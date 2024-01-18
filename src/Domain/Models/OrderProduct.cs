namespace GoodHamburger.Domain.Models
{
    public record OrderProduct : Entity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; } = new();
        public Order Order { get; set; } = new();
    }
}
