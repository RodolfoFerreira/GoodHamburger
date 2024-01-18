using GoodHamburger.Domain.Models;

namespace GoodHamburger.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll();
        Task<List<Product>> GetAllExtras();
        Task<List<Product>> GetAllByCategory(EnumCategory productCategory);
    }
}
