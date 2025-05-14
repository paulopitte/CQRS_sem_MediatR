using CQRS_sem_MediatR.Products.Exceptions;
using CQRS_sem_MediatR.Repositories;

namespace CQRS_sem_MediatR.Products.Commands.Handlers;

public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<DeleteProductCommandHandler> _logger;

    public DeleteProductCommandHandler(IProductRepository productRepository, 
                                       ILogger<DeleteProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task Handle(DeleteProductCommand request)
    {
        _logger.LogInformation(">>> Processando exclusão do produto {ProductId}.", request.Id);

        var deleted = await _productRepository.DeleteProductAsync(request.Id);

        if (!deleted)
        {
            _logger.LogWarning(">>> Não foi possível excluir produto com Id = {ProductId}.", request.Id);
            throw new CustomValidationException(new[] { $"Produto com ID {request.Id} não encontrado." });
        }
        
        _logger.LogWarning(">>> Produto com Id = {ProductId} excluído com sucesso", request.Id);
    }
}

