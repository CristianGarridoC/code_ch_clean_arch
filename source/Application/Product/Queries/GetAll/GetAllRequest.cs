using MediatR;

namespace Application.Product.Queries.GetAll;

public record GetAllRequest() : IRequest<GetAllResponse>;