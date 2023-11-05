using MediatR;

namespace Application.Product.Commands.Edit;

public record EditRequest(Guid Id, string Name, string Brand, decimal Price) : IRequest<Unit>;