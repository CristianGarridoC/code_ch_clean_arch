using Application.Abstractions;
using Application.Exceptions;
using Domain.Products;
using MediatR;

namespace Application.Product.Commands.Delete;

public class DeleteCommandHandler : IRequestHandler<DeleteRequest, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IPublisher _publisher;

    public DeleteCommandHandler(IProductRepository productRepository, IPublisher publisher)
    {
        _productRepository = productRepository;
        _publisher = publisher;
    }

    public async Task<Unit> Handle(DeleteRequest request, CancellationToken cancellationToken)
    {
        var productId = new ProductId(request.Id);
        var productInDb = await _productRepository.GetByIdAsync(productId);
        if (productInDb is null)
        {
            throw new NotFoundException(Constants.Product.NotFound);
        }
        await _productRepository.DeleteAsync(productId);
        await _publisher.Publish(new CacheInvalidationProductEvent(), cancellationToken);
        return Unit.Value;
    }
}