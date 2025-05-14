namespace CQRS_sem_MediatR.Products.Middlewares;

// Validação para comandos
public class CommandValidationMiddleware<TCommand> : IDispatcherCommandMiddleware<TCommand>
{
    private readonly IValidator<TCommand> _validator;
    private readonly ILogger<CommandValidationMiddleware<TCommand>> _logger;
    public CommandValidationMiddleware(
        IValidator<TCommand> validator,
        ILogger<CommandValidationMiddleware<TCommand>> logger)
    {
        _validator = validator;
        _logger = logger;
    }

    public async Task Handle(TCommand command, Func<Task> next)
    {
        _logger.LogDebug(">>> Validando comando {CommandType}", typeof(TCommand).Name);

        var result = await _validator.ValidateAsync(command);
        if (!result.IsValid)
        {
            _logger.LogWarning(">>> Validação falhou para {CommandType}", typeof(TCommand).Name);
            throw new CustomValidationException(result.Errors);
        }
        await next();
    }
}