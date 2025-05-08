namespace CQRS_sem_MediatR.Products.Queries.Handlers;

public sealed class GetAllProductsQueryHandler(IProductRepository productRepository) : IQueryHandler<GetAllProductsQuery, List<Product>>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<List<Product>> Handle(GetAllProductsQuery request)
    {
        return await _productRepository.GetAllProductsAsync();
    }
}
