namespace Domain.Models
{
    public record Category : Entity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = [];
    }

    public enum EnumCategory
    {
        Sandwich = 1,
        Beverage,
        Garnish
    }
}
