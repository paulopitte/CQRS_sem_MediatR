namespace CQRS_sem_MediatR.Products.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (CustomValidationException ex)
        {
            var statusCode = ex.Errors.Any(e => e.Contains("não encontrado"))
                ? StatusCodes.Status404NotFound
                : StatusCodes.Status400BadRequest;

            _logger.LogWarning("Erro de validação: {Errors}", string.Join("; ", ex.Errors));
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(new { Message = "Erro na operação", Errors = ex.Errors });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado na API.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { Message = "Erro interno no servidor" });
        }
    }
}