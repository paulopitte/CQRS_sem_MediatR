namespace CQRS_sem_MediatR.Products.Caching;

public class MemoryCacheService(ILogger<MemoryCacheService> logger, HybridCache hybridCache) : ICacheService
{
    private readonly HybridCache _hybridCache = hybridCache;
    private readonly ILogger<MemoryCacheService> _logger = logger;


    public async Task<T?> GetAsync<T>(string key)
    {
        var tags = new List<string> { key };
        T? result = default;

        result = await _hybridCache.GetOrCreateAsync(
         key,
         async cancelationToken =>
         {
             _logger.LogInformation($"[CACHE] Recuperando chave {key}");
             return await Task.FromResult((T)result!);
         },

         new HybridCacheEntryOptions
         {
             Expiration = TimeSpan.FromSeconds(20),
             LocalCacheExpiration = TimeSpan.FromSeconds(20),
             Flags = HybridCacheEntryFlags.None
         },
         tags);
        _logger.LogWarning($"[CACHE] Chave não encontrada: {key}");
        return await Task.FromResult(default(T));

    }


    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        _logger.LogInformation($"[CACHE] Armazenando chave {key} com expiração de {expiration.TotalMinutes} minutos");

        await _hybridCache.SetAsync(key, value,
              new HybridCacheEntryOptions
              {
                  Expiration = expiration, //Define expiração fixa
                  LocalCacheExpiration = TimeSpan.FromSeconds(20)
              },
              new[] { key });

        await Task.CompletedTask;
    }
}