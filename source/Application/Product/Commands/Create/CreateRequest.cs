using MediatR;
using Product.Application.Product.Commands.Create;

namespace Application.Product.Commands.Create;

public record CreateRequest(string Name, string Brand, decimal Price) : IRequest<CreateResponse>;