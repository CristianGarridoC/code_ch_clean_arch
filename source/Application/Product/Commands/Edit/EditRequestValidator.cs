using FluentValidation;

namespace Application.Product.Commands.Edit;

internal class EditRequestValidator : AbstractValidator<EditRequest>
{
    public EditRequestValidator()
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