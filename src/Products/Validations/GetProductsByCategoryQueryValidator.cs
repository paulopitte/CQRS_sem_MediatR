using CQRS_sem_MediatR.Products.Queries;
using CQRS_sem_MediatR.Repositories;

namespace CQRS_sem_MediatR.Products.Validations;

public class GetProductsByCategoryQueryValidator : 
             IValidator<GetProductsByCategoryQuery>
{
    private readonly IProductRepository _productRepository;

    public GetProductsByCategoryQueryValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ValidationResult> ValidateAsync(GetProductsByCategoryQuery request)
    {
        var errors = new List<string>();

        if (request.CategoryId <= 0)
            errors.Add("O ID da categoria deve ser um número positivo.");

        if (!await _productRepository.CategoryExistsAsync(request.CategoryId))
            errors.Add($"Categoria com ID {request.CategoryId} não encontrada.");

        return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
    }
}

