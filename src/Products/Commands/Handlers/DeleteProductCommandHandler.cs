namespace CQRS_sem_MediatR.Products.Commands.Handlers;

public sealed class DeleteProductCommandHandler(IProductRepository productRepository) : ICommandHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task Handle(DeleteProductCommand request) => await _productRepository.DeleteProductAsync(request.Id);

}
