using GoodHamburger.Domain.Models;

namespace GoodHamburger.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> Add(Order order);
        Task<Order> Update(Order order);
        Task<Order?> GetById(int id);
        Task<bool> Delete(int id);
        Task<List<Order>> GetAll();
    }
}
