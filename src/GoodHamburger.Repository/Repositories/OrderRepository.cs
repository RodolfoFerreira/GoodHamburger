using GoodHamburger.Domain.Models;
using GoodHamburger.Infrastructure.DatabaseContext;
using GoodHamburger.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Repository.Repositories
{
    public class OrderRepository(ApplicationDbContext context) : IOrderRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Order> Add(Order order)
        {
            await _context.Order.AddAsync(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<bool> Delete(int id)
        {
            var itemToRemove = await _context.Order.FirstAsync(x => x.Id == id);
            _context.Remove(itemToRemove);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Order>> GetAll() => await _context.Order
            .Include(x => x.OrderProducts)
            .ThenInclude(x => x.Product)
            .ThenInclude(x => x.Category)
            .ToListAsync();

        public async Task<Order?> GetById(int id) => await _context.Order
            .Include(x => x.OrderProducts)
            .ThenInclude(x => x.Product)
            .ThenInclude(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Order> Update(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return order;
        }
    }
}
