using FluentValidation;
using GoodHamburger.Domain.Models;

namespace GoodHamburger.Infrastructure.Validators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(x => x.OrderProducts)
                .Must(x =>
                {
                    var countSandwiches = x.Count(x => x.Product.CategoryId == (int)EnumCategory.Sandwich);
                    var countBeverages = x.Count(x => x.Product.CategoryId == (int)EnumCategory.Beverage);
                    var countGarnishes = x.Count(x => x.Product.CategoryId == (int)EnumCategory.Garnish);

                    return countSandwiches <= 1 && countBeverages <= 1 && countGarnishes <= 1;
                })
                .WithMessage("An order cannot contain more than one sandwich, beverage or garnish");
        }
    }
}
