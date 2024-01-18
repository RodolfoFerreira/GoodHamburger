using System.Collections.ObjectModel;

namespace GoodHamburger.Domain.Models
{
    public record Order : Entity
    {
        public string CustomerName { get; set; } = string.Empty;
        public double DiscountPerc { get; set; }
        public double DiscountValue { get; set; }
        public double OrderValue { get; set; }
        public double FinalValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Collection<OrderProduct> OrderProducts { get; set; } = [];
    }
}
