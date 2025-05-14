namespace CQRS_sem_MediatR.Products.Queries.Handlers;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Product?>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetProductByIdQueryHandler> _logger;

    public GetProductByIdQueryHandler(IProductRepository productRepository, ILogger<GetProductByIdQueryHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Product?> Handle(GetProductByIdQuery request)
    {
        _logger.LogInformation(">>> Consultando o produto {ProductId}.", request.Id);

        var product = await _productRepository.GetProductByIdAsync(request.Id);

        if (product is null)
        {
            _logger.LogWarning(">>> Produto {ProductId} não encontrado.", request.Id);
            throw new CustomValidationException(new[] { $"Produto com ID {request.Id} não encontrado." });
        }
        return product;
    }
}