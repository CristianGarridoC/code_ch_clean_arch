using Application.Abstractions;
using Application.Exceptions;
using Domain.Products;
using MediatR;

namespace Application.Product.Commands.Edit;

public class EditCommandHandler : IRequestHandler<EditRequest, Unit>
{
    private readonly IProductRepository _productRepository;

    public EditCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Unit> Handle(EditRequest request, CancellationToken cancellationToken)
    {
        var productId = new ProductId(request.Id);
        var checkProductId = await _productRepository.GetByIdAsync(productId);
        if (checkProductId is null)
        {
            throw new NotFoundException(Constants.Product.NotFound);
        }
        
        var name = Name.Create(request.Name);
        var brand = Brand.Create(request.Brand);
        var checkProductName = await _productRepository.GetByNameAndBrandAsync(name, brand, productId);
        if (checkProductName is not null)
        {
            throw new AlreadyExistsException(Constants.Product.AlreadyExists);
        }
        
        var price = Money.Create(request.Price, Currency.USD);
        var product = Domain.Products.Product.Create(productId, name, brand, price);

        await _productRepository.UpdateAsync(product);
        return Unit.Value;
    }
}