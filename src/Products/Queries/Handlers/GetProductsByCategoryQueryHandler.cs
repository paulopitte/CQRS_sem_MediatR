namespace CQRS_sem_MediatR.Products.Queries.Handlers;

public class GetProductsByCategoryQueryHandler : IQueryHandler<GetProductsByCategoryQuery, List<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetProductsByCategoryQueryHandler> _logger;

    public GetProductsByCategoryQueryHandler(IProductRepository productRepository, ILogger<GetProductsByCategoryQueryHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<List<Product>> Handle(GetProductsByCategoryQuery request)
    {
        _logger.LogInformation(">>> Consultando produtos da categoria {CategoryId}.", request.CategoryId);

        var products = await _productRepository.GetProductsByCategoryAsync(request.CategoryId);

        if (products is null || !products.Any()) 
        {
            _logger.LogWarning(">>> Nenhum produto encontrado na categoria {CategoryId}.", request.CategoryId);
            return new List<Product>(); // Retorna lista vazia corretamente
        }
        return products;
    }
}

