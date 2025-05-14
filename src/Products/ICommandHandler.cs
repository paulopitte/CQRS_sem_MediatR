namespace CQRS_sem_MediatR.Products;

public interface ICommandHandler<TCommand> where TCommand : class
{
    Task Handle(TCommand command);
}
