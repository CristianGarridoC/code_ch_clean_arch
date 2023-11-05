using MediatR;

namespace Application.Product.Commands.Delete;

public record DeleteRequest(Guid Id): IRequest<Unit>;