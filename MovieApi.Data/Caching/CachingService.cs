using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using MovieApi.Domain.Interfaces;

namespace MovieApi.Data.Caching;

public class CachingService : ICachingService
{
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions _cacheOptions;
    
    public CachingService(IDistributedCache cache)
    {
        _cache = cache;
        _cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            SlidingExpiration = TimeSpan.FromMinutes(2)
        };
    }
    
    public async Task SetAsync(string key, string value)
    {
        await _cache.SetStringAsync(key, value, _cacheOptions);
    }

    public async  Task<string> GetAsync(string key)
    {
        return await _cache.GetStringAsync(key);
    }
}