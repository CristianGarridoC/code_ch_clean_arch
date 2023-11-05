using Application;
using Application.Abstractions;
using Application.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Cache;

internal sealed class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<CacheService> _logger;

    public CacheService(IDistributedCache cache, ILogger<CacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task SetRecordAsync(string recordName, string data, DistributedCacheEntryOptions options)
    {
        try
        {
            await _cache.SetStringAsync(recordName, data, options);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "There was an error in {MethodName}", nameof(SetRecordAsync));
            throw new CacheException(Constants.Cache.FailedCacheKey);
        }
    }

    public async Task<string?> GetRecordAsync(string recordName)
    {
        try
        {
            return await _cache.GetStringAsync(recordName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "There was an error in {MethodName}", nameof(GetRecordAsync));
            throw new CacheException(Constants.Cache.FailedGetKey);
        }
    }

    public async Task DeleteRecord(string recordName)
    {
        try
        {
            await _cache.RemoveAsync(recordName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "There was an error in {MethodName}", nameof(DeleteRecord));
            throw new CacheException(Constants.Cache.FailedDeleteKey);
        }
    }
}