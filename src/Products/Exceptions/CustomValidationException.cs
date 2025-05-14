namespace CQRS_sem_MediatR.Products.Exceptions;

public class CustomValidationException : Exception
{
    public List<string> Errors { get; }

    public CustomValidationException(IEnumerable<string> errors)
        : base(string.Join("; ", errors)) // Converte lista de erros para string
    {
        Errors = errors.ToList();
    }
}
