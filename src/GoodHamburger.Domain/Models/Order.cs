using System.Collections.ObjectModel;

namespace GoodHamburger.Domain.Models
{
    public record Order : Entity
    {
        public string CustomerName { get; set; } = string.Empty;
        public double DiscountPerc { get; set; }
        public double DiscountValue { get; set; }
        public double GrossValue { get; set; }
        public double NetValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Collection<OrderProduct> OrderProducts { get; set; } = [];
    }
}
