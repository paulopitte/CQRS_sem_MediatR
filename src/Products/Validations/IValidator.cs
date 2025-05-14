namespace CQRS_sem_MediatR.Products.Validations;

public interface IValidator<T>
{
    Task<ValidationResult> ValidateAsync(T request);
}
