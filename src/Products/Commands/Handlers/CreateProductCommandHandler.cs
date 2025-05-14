using CQRS_sem_MediatR.Entities;
using CQRS_sem_MediatR.Products.Exceptions;
using CQRS_sem_MediatR.Repositories;

namespace CQRS_sem_MediatR.Products.Commands.Handlers;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(IProductRepository productRepository, 
                                       ILogger<CreateProductCommandHandler> logger = null!)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task Handle(CreateProductCommand request)
    {
        if (request is null)
        {
            _logger.LogError(">>>> Erro ao criar produto: request é nulo.");
            throw new CustomValidationException(new[] { $"O comando de criação não pode ser nulo." });
        }

        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock,
            CategoryId = request.CategoryId,
            Active = request.Active
        };

        var createdProduct = await _productRepository.CreateProductAsync(product);
        _logger.LogInformation(">>> Produto criado com sucesso: {ProductId}, {ProductName}", createdProduct.Id, createdProduct.Name);
    }
} 
