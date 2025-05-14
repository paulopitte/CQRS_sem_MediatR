using CQRS_sem_MediatR.Products.Commands;
using CQRS_sem_MediatR.Repositories;

namespace CQRS_sem_MediatR.Products.Validations;

public class DeleteProductCommandValidator : IValidator<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;
    public DeleteProductCommandValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<ValidationResult> ValidateAsync(DeleteProductCommand request)
    {
        var errors = new List<string>();

        if (request.Id <= 0)
            errors.Add("O ID do produto deve ser um número positivo.");

        var product = await _productRepository.GetProductByIdAsync(request.Id);

        if (product is null)
            errors.Add($"Produto com ID {request.Id} não encontrado.");

        return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
    }
}
