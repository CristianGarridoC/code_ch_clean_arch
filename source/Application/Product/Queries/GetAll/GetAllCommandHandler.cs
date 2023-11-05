using Application.Abstractions;
using Application.Product.Common;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Product.Queries.GetAll;

public class GetAllCommandHandler : IRequestHandler<GetAllRequest, GetAllResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ICacheService _cacheService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<GetAllCommandHandler> _logger;

    public GetAllCommandHandler(IProductRepository productRepository,
        ICacheService cacheService, IDateTimeProvider dateTimeProvider, ILogger<GetAllCommandHandler> logger)
    {
        _productRepository = productRepository;
        _cacheService = cacheService;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<GetAllResponse> Handle(GetAllRequest request, CancellationToken cancellationToken)
    {
        var result = Enumerable.Empty<ProductResponse>();
        var cache = await _cacheService.GetRecordAsync("GetAll_Products");
        if (string.IsNullOrWhiteSpace(cache))
        {
            _logger.LogInformation("Not using cache in GetAll command");
            result = await _productRepository.GetAll();
            await _cacheService.SetRecordAsync(
                "GetAll_Products",
                JsonConvert.SerializeObject(result),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = _dateTimeProvider.DateTimeOffsetNow.AddSeconds(20)
                });
            return new GetAllResponse(result);
        }
        _logger.LogInformation("Using cache in GetAll command");
        result = JsonConvert.DeserializeObject<IEnumerable<ProductResponse>>(cache);
        return new GetAllResponse(result);
    }
}