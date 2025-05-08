namespace CQRS_sem_MediatR.Products.Queries.Handlers;

public sealed class GetProductsByCategoryQueryHandler(IProductRepository productRepository) : IQueryHandler<GetProductsByCategoryQuery, List<Product>>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<List<Product>> Handle(GetProductsByCategoryQuery request)
    {
        return await _productRepository.GetProductsByCategoryAsync(request.CategoryId);
    }
}
