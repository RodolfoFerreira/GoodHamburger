using FluentValidation;
using GoodHamburger.Domain.DTO.Order;

namespace GoodHamburger.Infrastructure.Validators
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer name is required")
                .MaximumLength(100).WithMessage("Customer name must have a maximum of 100 characters");

            RuleForEach(x => x.OrderProducts).SetValidator(new CreateOrderProductDtoValidator());

            RuleFor(x => x.OrderProducts)
                .NotEmpty()
                .WithMessage("Order products are required")
                .Must(x => !x.GroupBy(x => x.ProductId).Select(g => new { id = g.Key, count = g.Count() }).Any(a => a.count > 1))
                .WithMessage("An order can't have any repeated products");                

        }
    }
}
