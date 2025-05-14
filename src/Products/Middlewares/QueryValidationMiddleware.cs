using System.Reflection;

namespace CQRS_sem_MediatR.Products.Middlewares;

public class QueryValidationMiddleware<TQuery, TResult> : IDispatcherQueryMiddleware<TQuery, TResult>
{
    private readonly IValidator<TQuery>? _validator;
    private readonly ILogger<QueryValidationMiddleware<TQuery, TResult>> _logger;

    public QueryValidationMiddleware(
        ILogger<QueryValidationMiddleware<TQuery, TResult>> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;

        // Verifica se a query tem propriedades antes de tentar resolver o validador
        if (HasProperties(typeof(TQuery)))
        {
            _validator = serviceProvider.GetService<IValidator<TQuery>>();
        }
    }

    public async Task<TResult> Handle(TQuery query, Func<Task<TResult>> next)
    {
        if (_validator != null)
        {
            _logger.LogDebug(">>> Validando consulta {QueryType}", typeof(TQuery).Name);

            var result = await _validator.ValidateAsync(query);
            if (!result.IsValid)
            {
                _logger.LogWarning(">>> Validação falhou para {QueryType}", typeof(TQuery).Name);
                throw new CustomValidationException(result.Errors);
            }
        }
        else
        {
            _logger.LogInformation(">>> Consulta não tem parâmetros (Sem validação)");
        }

        return await next();
    }

    private static bool HasProperties(Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   .Any(p => p.CanRead);
    }
}