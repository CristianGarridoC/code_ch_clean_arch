using FluentValidation;

namespace Application.Product.Commands.Delete;

internal class DeleteRequestValidator : AbstractValidator<DeleteRequest>
{
    public DeleteRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}