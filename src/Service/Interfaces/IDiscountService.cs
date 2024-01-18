using GoodHamburger.Domain.Models;

namespace GoodHamburger.Service.Interfaces
{
    public interface IDiscountService
    {
        Order CalculateOrderDiscount(Order order);
    }
}
