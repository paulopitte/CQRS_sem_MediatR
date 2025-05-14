namespace CQRS_sem_MediatR.Products.Middlewares;

public interface IDispatcherCommandMiddleware<TRequest>
{
    Task Handle(TRequest request, Func<Task> next);
}
