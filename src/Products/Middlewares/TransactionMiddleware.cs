using CQRS_sem_MediatR.Data.Context;

namespace CQRS_sem_MediatR.Products.Middlewares;

public class TransactionMiddleware<TCommand> : IDispatcherCommandMiddleware<TCommand>
{
    private readonly AppDbContext _context;
    private readonly ILogger<TransactionMiddleware<TCommand>> _logger;
    public TransactionMiddleware(
        AppDbContext context,
        ILogger<TransactionMiddleware<TCommand>> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task Handle(TCommand command, Func<Task> next)
    {
        _logger.LogInformation(">>> Iniciando transação para {CommandType}", typeof(TCommand).Name);

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await next();
            await transaction.CommitAsync();

            _logger.LogInformation(">>> Transação concluída com sucesso para {CommandType}",
                typeof(TCommand).Name);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            _logger.LogError(">>> Transação revertida para {CommandType}", typeof(TCommand).Name);
            throw;
        }
    }
}