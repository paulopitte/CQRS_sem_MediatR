using System.Diagnostics;

namespace CQRS_sem_MediatR.Products.Middlewares;

// Logging para consultas
public class QueryLoggingMiddleware<TQuery, TResult> : IDispatcherQueryMiddleware<TQuery, TResult>
{
    private readonly ILogger<QueryLoggingMiddleware<TQuery, TResult>> _logger;

    public QueryLoggingMiddleware(ILogger<QueryLoggingMiddleware<TQuery, TResult>> logger)
    {
        _logger = logger;
    }

    public async Task<TResult> Handle(TQuery query, Func<Task<TResult>> next)
    {
        var queryName = typeof(TQuery).Name;

        _logger.LogInformation("[INÍCIO] Processando consulta {QueryName}", queryName);
        _logger.LogDebug("Parâmetros da consulta: {@Query}", query);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var result = await next();
            stopwatch.Stop();

            _logger.LogInformation("[SUCESSO] Consulta {QueryName} concluída em {ElapsedMs}ms",
                queryName, stopwatch.ElapsedMilliseconds);
            _logger.LogDebug("Resultado: {@Result}", result);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "[ERRO] Consulta {QueryName} falhou após {ElapsedMs}ms",
                queryName, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}
