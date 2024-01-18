using GoodHamburger.Domain.DTO.Product;

namespace GoodHamburger.Domain.DTO.OrderProduct
{
    public class OrderProductDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public ProductDto Product { get; set; } = new();
    }
}
