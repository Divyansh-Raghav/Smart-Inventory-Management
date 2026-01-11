using FluentValidation;
using SmartInventoryManager.Api.DTOs;

namespace SmartInventoryManager.Api.Validators
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.CustomerId).GreaterThan(0);
            RuleFor(x => x.OrderItems).NotEmpty().WithMessage("Order must have items");
            RuleForEach(x => x.OrderItems).ChildRules(items =>
            {
                items.RuleFor(item => item.ProductId).GreaterThan(0);
                items.RuleFor(item => item.Quantity).GreaterThan(0);
            });
        }
    }
}
