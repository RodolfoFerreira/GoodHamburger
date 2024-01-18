using GoodHamburger.Domain.DTO.Category;

namespace GoodHamburger.Domain.DTO.Product
{
    public record ProductDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public bool IsExtra { get; set; }
        public decimal Price { get; set; }
        public CategoryDto Category { get; set; } = new();
    }
}
