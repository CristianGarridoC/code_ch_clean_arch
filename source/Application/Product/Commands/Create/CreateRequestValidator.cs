using FluentValidation;

namespace Application.Product.Commands.Create;

internal class CreateRequestValidator : AbstractValidator<CreateRequest>
{
    public CreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Brand)
            .NotEmpty();

        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThan(0);
    }
}