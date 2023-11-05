using Application.Abstractions;
using Application.Exceptions;
using Domain.Products;
using MediatR;
using Product.Application.Product.Commands.Create;

namespace Application.Product.Commands.Create;

public class CreateCommandHandler : IRequestHandler<CreateRequest, CreateResponse>
{
    private readonly IProductRepository _productRepository;

    public CreateCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<CreateResponse> Handle(CreateRequest request, CancellationToken cancellationToken)
    {
        var name = Name.Create(request.Name);
        var price = Money.Create(request.Price, Currency.USD);
        var brand = Brand.Create(request.Brand);
        var product = Domain.Products.Product.Create(name, brand, price);

        var productInDb = await _productRepository.GetByNameAndBrandAsync(product.Name, product.Brand, product.Id);
        if (productInDb is not null)
        {
            throw new AlreadyExistsException(Constants.Product.AlreadyExists);
        }

        await _productRepository.AddAsync(product);

        return new CreateResponse(product.Id.Value.ToString());
    }
}