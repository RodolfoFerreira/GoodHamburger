using System.Collections.ObjectModel;

namespace GoodHamburger.Domain.Models
{
    public record Product : Entity
    {
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public bool IsExtra { get; set; }
        public double Price { get; set; }
        public Category Category { get; set; } = new();
        public Collection<OrderProduct> OrderProducts { get; set; } = [];
    }
}
