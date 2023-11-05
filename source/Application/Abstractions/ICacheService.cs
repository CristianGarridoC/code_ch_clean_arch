using Microsoft.Extensions.Caching.Distributed;

namespace Application.Abstractions;

public interface ICacheService
{
    Task SetRecordAsync(string recordName, string data, DistributedCacheEntryOptions options);
    Task<string?> GetRecordAsync(string recordName);
    Task DeleteRecord(string recordName);
}