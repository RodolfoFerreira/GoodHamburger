using GoodHamburger.Domain.Models;
using GoodHamburger.Service.Interfaces;

namespace GoodHamburger.Service.Services
{
    public class DiscountService : IDiscountService
    {
        public Order CalculateOrderDiscount(Order order)
        {
            if (order == null) { return new Order(); }

            var countSandwitch = order.OrderProducts.Count(x => x.Product.CategoryId == (int)EnumCategory.Sandwich);
            var countBeverage = order.OrderProducts.Count(x => x.Product.CategoryId == (int)EnumCategory.Beverage);
            var countGarnish = order.OrderProducts.Count(x => x.Product.CategoryId == (int)EnumCategory.Garnish);

            if (countSandwitch >= 1 && countBeverage >= 1 && countGarnish >= 1)
                return CalculateDiscount(order, 20);

            if (countSandwitch >= 1 && countBeverage >= 1)
                return CalculateDiscount(order, 15);

            if (countSandwitch >= 1 && countGarnish >= 1)
                return CalculateDiscount(order, 10);

            return CalculateDiscount(order, 0);
        }

        private static Order CalculateDiscount(Order order, double discountPerc)
        {
            order.DiscountPerc = discountPerc;
            order.DiscountValue = Math.Round(order.GrossValue * discountPerc / 100, 2);
            order.NetValue = order.GrossValue - order.DiscountValue;

            return order;
        }
    }
}
