namespace CQRS_sem_MediatR.Products.Commands.Handlers;

public sealed class CreateProductCommandHandler(IProductRepository productRepository) : ICommandHandler<CreateProductCommand>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task Handle(CreateProductCommand request)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock,
            CategoryId = request.CategoryId,
            Active = request.Active
        };

        await _productRepository.CreateProductAsync(product);
    }
} 
