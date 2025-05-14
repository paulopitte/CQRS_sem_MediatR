using CQRS_sem_MediatR.Products.Middlewares;

namespace CQRS_sem_MediatR.Products;

public class Dispatcher : IDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Dispatcher> _logger;

    public Dispatcher(IServiceProvider serviceProvider, ILogger<Dispatcher> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task Dispatch<TCommand>(TCommand command) where TCommand : class
    {
        _logger.LogInformation($"[DISPATCHER] Obter instância do handler para processar o comando : {command} ");
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();

        // Constroi pipeline para comandos
        Func<Task> pipeline = () => handler.Handle(command);

        var middlewares = _serviceProvider.GetServices<IDispatcherCommandMiddleware<TCommand>>()
            .Reverse();

        foreach (var middleware in middlewares)
        {
            var current = middleware;
            var next = pipeline;
            pipeline = () => current.Handle(command, next);
        }

        await pipeline();
    }

    public async Task<TResult> Dispatch<TQuery, TResult>(TQuery query) where TQuery : class
    {
        _logger.LogInformation($"[DISPATCHER] Obter instância do handler para processar a consulta : {query} ");
        var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();

        // Constroi pipeline para consultas
        Func<Task<TResult>> pipeline = () => handler.Handle(query);

        var middlewares = _serviceProvider.GetServices<IDispatcherQueryMiddleware<TQuery, TResult>>()
            .Reverse();

        foreach (var middleware in middlewares)
        {
            var current = middleware;
            var next = pipeline;
            pipeline = () => current.Handle(query, next);
        }

        return await pipeline();
    }
}