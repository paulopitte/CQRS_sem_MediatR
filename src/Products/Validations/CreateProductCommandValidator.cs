namespace CQRS_sem_MediatR.Products.Validations;

public class CreateProductCommandValidator : IValidator<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;
    public CreateProductCommandValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<ValidationResult> ValidateAsync(CreateProductCommand request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Name))
            errors.Add("O nome do produto não pode estar vazio.");

        if (request.Price <= 0)
            errors.Add("O preço deve ser maior que zero.");

        if (request.Stock <= 0)
            errors.Add("O estoque tem que zer maior que zero.");

        if (request.CategoryId <= 0)
            errors.Add("O Id da categoria tem que ser maior que zero.");

        if (!await _productRepository.CategoryExistsAsync(request.CategoryId))
            errors.Add($"Categoria com ID {request.CategoryId} não encontrada.");

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var existingProduct = await _productRepository.GetProductByNameAsync(request.Name);

            if (existingProduct is not null)
                errors.Add("Já existe um produto com esse nome.");
        }

        // Verifica se a lista de erros (errors) contém algum erro.
        // Se sim, retorna um ValidationResult indicando falha e contendo a lista de erros.
        // Caso contrário (se a lista de erros estiver vazia), retorna um ValidationResult
        // indicando sucesso
        return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
    }
}
