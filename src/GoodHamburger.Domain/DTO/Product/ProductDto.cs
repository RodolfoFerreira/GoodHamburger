using GoodHamburger.Domain.DTO.Category;

namespace GoodHamburger.Domain.DTO.Product
{
    public record ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public bool IsExtra { get; set; }
        public double Price { get; set; }
        public CategoryDto Category { get; set; } = new();
    }
}
