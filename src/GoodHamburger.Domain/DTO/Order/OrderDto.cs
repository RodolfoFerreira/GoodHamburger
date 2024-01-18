using GoodHamburger.Domain.DTO.OrderProduct;

namespace GoodHamburger.Domain.DTO.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public double DiscountPerc { get; set; }
        public double DiscountValue { get; set; }
        public double GrossValue { get; set; }
        public double NetValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderProductDto> OrderProducts { get; set; } = [];
    }
}
