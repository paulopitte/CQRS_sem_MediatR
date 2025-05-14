namespace CQRS_sem_MediatR.Products;

public interface IDispatcher
{
    Task Dispatch<TCommand>(TCommand command) where TCommand : class;
    Task<TResult> Dispatch<TQuery, TResult>(TQuery query) where TQuery : class;
}
