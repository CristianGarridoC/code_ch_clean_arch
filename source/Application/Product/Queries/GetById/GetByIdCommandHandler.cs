using Application.Abstractions;
using Application.Exceptions;
using Domain.Products;
using MediatR;

namespace Application.Product.Queries.GetById;

public class GetByIdCommandHandler : IRequestHandler<GetByIdRequest, Common.ProductResponse>
{
    private readonly IProductRepository _productRepository;

    public GetByIdCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Common.ProductResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)
    {
        var productId = new ProductId(request.Id);
        var result = await _productRepository.GetByIdAsync(productId);
        if (result is null)
        {
            throw new NotFoundException(Constants.Product.NotFound);
        }

        return result;
    }
}