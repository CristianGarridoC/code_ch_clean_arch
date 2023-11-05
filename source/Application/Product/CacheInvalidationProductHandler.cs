using Application.Abstractions;
using MediatR;

namespace Application.Product;

internal sealed class CacheInvalidationProductHandler : INotificationHandler<CacheInvalidationProductEvent>
{
    private readonly ICacheService _cacheService;

    public CacheInvalidationProductHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task Handle(CacheInvalidationProductEvent notification, CancellationToken cancellationToken)
    {
        await _cacheService.DeleteRecord(Constants.Product.CacheKey);
    }
}