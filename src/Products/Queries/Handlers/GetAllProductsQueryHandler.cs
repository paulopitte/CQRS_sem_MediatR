namespace CQRS_sem_MediatR.Products.Queries.Handlers;

public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, List<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetAllProductsQueryHandler> _logger;

    public GetAllProductsQueryHandler(IProductRepository productRepository, ILogger<GetAllProductsQueryHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<List<Product>> Handle(GetAllProductsQuery request)
    {
        _logger.LogInformation(">>> Consultando todos os produtos.");

        var products = await _productRepository.GetAllProductsAsync();

        if (products is null || !products.Any())
        {
            _logger.LogWarning("{AVISO] Nenhum produto encontrado.");
            return new List<Product>(); // Retorna lista vazia de forma segura
        }
        return products;
    }
}

