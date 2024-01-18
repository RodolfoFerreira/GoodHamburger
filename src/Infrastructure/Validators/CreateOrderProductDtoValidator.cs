using FluentValidation;
using GoodHamburger.Domain.DTO.OrderProduct;

namespace GoodHamburger.Infrastructure.Validators
{
    public class CreateOrderProductDtoValidator : AbstractValidator<CreateOrderProductDto>
    {
        public CreateOrderProductDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product id is required");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }
}
