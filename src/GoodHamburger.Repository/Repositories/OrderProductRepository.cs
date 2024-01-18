using GoodHamburger.Infrastructure.DatabaseContext;
using GoodHamburger.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Repository.Repositories
{
    public class OrderProductRepository(ApplicationDbContext context) : IOrderProductRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<int> DeleteAllById(IEnumerable<int> ids)
        {
            var itemsToRemove = await _context.OrderProduct
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            _context.RemoveRange(itemsToRemove);

            return await _context.SaveChangesAsync();
        }
    }
}
