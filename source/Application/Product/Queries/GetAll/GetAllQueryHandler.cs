using Application.Abstractions;
using Application.Product.Common;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Product.Queries.GetAll;

public class GetAllQueryHandler : IRequestHandler<GetAllRequest, GetAllResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ICacheService _cacheService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<GetAllQueryHandler> _logger;

    public GetAllQueryHandler(IProductRepository productRepository,
        ICacheService cacheService, IDateTimeProvider dateTimeProvider, ILogger<GetAllQueryHandler> logger)
    {
        _productRepository = productRepository;
        _cacheService = cacheService;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<GetAllResponse> Handle(GetAllRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<ProductResponse> result;
        var cache = await _cacheService.GetRecordAsync(Constants.Product.CacheKey);
        if (!string.IsNullOrWhiteSpace(cache))
        {
            _logger.LogInformation("Using cache in GetAll command");
            result = JsonConvert.DeserializeObject<IEnumerable<ProductResponse>>(cache)!;
            return new GetAllResponse(result);
        }
        
        _logger.LogInformation("Not using cache in GetAll command");
        result = await _productRepository.GetAll();
        await _cacheService.SetRecordAsync(
            Constants.Product.CacheKey,
            JsonConvert.SerializeObject(result),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = _dateTimeProvider.DateTimeOffsetNow.AddHours(1)
            });
        return new GetAllResponse(result);
    }
}