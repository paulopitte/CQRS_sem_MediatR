using System.Diagnostics;

namespace CQRS_sem_MediatR.Products.Middlewares;

// Logging para comandos
public class CommandLoggingMiddleware<TCommand> : IDispatcherCommandMiddleware<TCommand>
{
    private readonly ILogger<CommandLoggingMiddleware<TCommand>> _logger;
    public CommandLoggingMiddleware(ILogger<CommandLoggingMiddleware<TCommand>> logger)
    {
        _logger = logger;
    }
    public async Task Handle(TCommand command, Func<Task> next)
    {
        var commandName = typeof(TCommand).Name;
        _logger.LogInformation("[INÍCIO] Processando comando {CommandName}", commandName);
        _logger.LogDebug("Dados do comando: {@Command}", command);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            await next();
            stopwatch.Stop();

            _logger.LogInformation("[SUCESSO] Comando {CommandName} concluído em {ElapsedMs}ms",
                commandName, stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "[ERRO] Comando {CommandName} falhou após {ElapsedMs}ms",
                commandName, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}