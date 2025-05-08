namespace CQRS_sem_MediatR.Products.Commands.Handlers;

public sealed class UpdateProductCommandHandler(IProductRepository productRepository) : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task Handle(UpdateProductCommand request)
    {
        var existingProduct = await _productRepository.GetProductByIdAsync(request.Id);
        if (existingProduct == null)
            throw new Exception("Product not found");

        existingProduct.Name = request.Name;
        existingProduct.Price = request.Price;
        existingProduct.Stock = request.Stock;
        existingProduct.CategoryId = request.CategoryId;
        existingProduct.Active = request.Active;

        await _productRepository.UpdateProductAsync(existingProduct);
    }
}
