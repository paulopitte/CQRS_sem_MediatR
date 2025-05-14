using CQRS_sem_MediatR.Repositories;

namespace CQRS_sem_MediatR.Products.Commands.Handlers;

public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<UpdateProductCommandHandler> _logger;

    public UpdateProductCommandHandler(IProductRepository productRepository, ILogger<UpdateProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task Handle(UpdateProductCommand request)
    {
        _logger.LogInformation(">>> Processando atualização do produto {ProductId}.", request.Id);

        var existingProduct = await _productRepository.GetProductByIdAsync(request.Id);

        if (existingProduct is null)
        {
            _logger.LogWarning(">>> Produto não encontrado: {ProductId}.", request.Id);
            return; 
        }

        existingProduct.Name = request.Name;
        existingProduct.Price = request.Price;
        existingProduct.Stock = request.Stock;
        existingProduct.CategoryId = request.CategoryId;
        existingProduct.Active = request.Active;

        await _productRepository.UpdateProductAsync(existingProduct);

        _logger.LogInformation(">>> Produto {ProductId} atualizado com sucesso.", request.Id);
    }
}
