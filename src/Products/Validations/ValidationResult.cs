namespace CQRS_sem_MediatR.Products.Validations;

public class ValidationResult
{
    public bool IsValid { get; }
    public List<string> Errors { get; } = new();

    private ValidationResult(bool isValid)
    {
        IsValid = isValid;
    }

    public static ValidationResult Success() => new(true);
    public static ValidationResult Failure(IEnumerable<string> errors)
    {
        var result = new ValidationResult(false);
        result.Errors.AddRange(errors);
        return result;
    }
}
