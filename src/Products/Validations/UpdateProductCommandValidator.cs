using CQRS_sem_MediatR.Products.Commands;
using CQRS_sem_MediatR.Repositories;

namespace CQRS_sem_MediatR.Products.Validations;

public class UpdateProductCommandValidator : IValidator<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    public UpdateProductCommandValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<ValidationResult> ValidateAsync(UpdateProductCommand request)
    {
        var errors = new List<string>();

        if (request.Id <= 0)
            errors.Add("O ID do produto deve ser um número positivo.");

        var product = await _productRepository.GetProductByIdAsync(request.Id);

        if (product is null)
            errors.Add($"Produto com ID {request.Id} não encontrado.");

        if (request.CategoryId <= 0)
            errors.Add("O Id da categoria tem que ser maior que zero.");

        if (!await _productRepository.CategoryExistsAsync(request.CategoryId))
            errors.Add($"Categoria com ID {request.CategoryId} não encontrada.");

        if (string.IsNullOrWhiteSpace(request.Name))
            errors.Add("O nome do produto não pode estar vazio.");

        if (request.Price <= 0)
            errors.Add("O preço deve ser maior que zero.");

        if (request.Stock <= 0)
            errors.Add("O estoque tem que ser maior que zero.");

        // Validação assíncrona: verificar se o nome já existe no banco
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var existingProduct = await _productRepository.GetProductByNameAsync(request.Name);

            if (existingProduct is not null)
                errors.Add("Já existe um produto com esse nome.");
        }
        return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
    }
}
