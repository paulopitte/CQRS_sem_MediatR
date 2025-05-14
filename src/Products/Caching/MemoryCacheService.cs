namespace CQRS_sem_MediatR.Products.Caching;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<MemoryCacheService> _logger;

    public MemoryCacheService(IMemoryCache cache, ILogger<MemoryCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public Task<T?> GetAsync<T>(string key)
    {
        if (_cache.TryGetValue(key, out T? value))
        {
            _logger.LogInformation($"[CACHE] Recuperando chave {key}");
            return Task.FromResult(value);
        }

        _logger.LogWarning($"[CACHE] Chave não encontrada: {key}");
        return Task.FromResult(default(T));
    }

    //public Task SetAsync<T>(string key, T value, TimeSpan expiration)
    //{
    //    _cache.Set(key, value, expiration);
    //    return Task.CompletedTask;
    //}

    public Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        _logger.LogInformation($"[CACHE] Armazenando chave {key} com expiração de {expiration.TotalMinutes} minutos");

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(expiration) // Define expiração fixa
            .SetPriority(CacheItemPriority.High); // Evita remoção prematura

        _cache.Set(key, value, cacheEntryOptions);
        return Task.CompletedTask;
    }
}