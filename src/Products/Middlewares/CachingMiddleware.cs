using CQRS_sem_MediatR.Products.Caching;
using System.Text.Json;

namespace CQRS_sem_MediatR.Products.Middlewares;

public class CachingMiddleware<TRequest, TResponse> : IDispatcherQueryMiddleware<TRequest, TResponse>
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<CachingMiddleware<TRequest, TResponse>> _logger;
    public CachingMiddleware(ICacheService cacheService, 
        ILogger<CachingMiddleware<TRequest, TResponse>> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, Func<Task<TResponse>> next)
    {
        //var cacheKey = $"{typeof(TRequest).Name}:{request.GetHashCode()}";
        //var cacheKey = $"{typeof(TRequest).Name}:{JsonConvert.SerializeObject(request)}";
        _logger.LogInformation($">>> Verificando a existência do CACHE");
        var cacheKey = $"{typeof(TRequest).Name}:{JsonSerializer.Serialize(request)}";

        var cachedResponse = await _cacheService.GetAsync<TResponse>(cacheKey);

        if (cachedResponse is not null)
        {
            _logger.LogInformation($">>> Criando o Cache para {cacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogInformation($">>> Não existe cache para {cacheKey}, processando requisição.");
        var response = await next(); // chamamos `next()` sem passar `request`

        await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(10)); 
        return response;
    }
}
