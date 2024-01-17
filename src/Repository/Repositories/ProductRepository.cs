using Domain.Models;
using Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository.Repositories
{
    internal class ProductRepository(ApplicationDbContext context) : IProductRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<Product>> GetAll() => await _context.Product.Include(x => x.Category).ToListAsync();

        public async Task<List<Product>> GetAllExtras() => await _context.Product.Include(x => x.Category).Where(x => x.IsExtra == true).ToListAsync();

        public async Task<List<Product>> GetAllByCategory(EnumCategory productCategory) => await _context.Product.Include(x => x.Category).Where(x => x.CategoryId == (int)productCategory).ToListAsync();
    }
}
