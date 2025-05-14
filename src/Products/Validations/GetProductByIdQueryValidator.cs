using CQRS_sem_MediatR.Products.Queries;
using CQRS_sem_MediatR.Repositories;

namespace CQRS_sem_MediatR.Products.Validations;

public class GetProductByIdQueryValidator : IValidator<GetProductByIdQuery>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ValidationResult> ValidateAsync(GetProductByIdQuery request)
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
