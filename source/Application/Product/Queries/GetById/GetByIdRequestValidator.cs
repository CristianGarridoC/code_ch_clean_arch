using FluentValidation;

namespace Application.Product.Queries.GetById;

internal class GetByIdRequestValidator : AbstractValidator<GetByIdRequest>
{
    public GetByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}