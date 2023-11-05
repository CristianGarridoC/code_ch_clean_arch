using MediatR;

namespace Application.Product.Queries.GetById;

public record GetByIdRequest(Guid Id) : IRequest<Common.ProductResponse>;