namespace CQRS_sem_MediatR.Products;

public interface IQueryHandler<TQuery, TResult> where TQuery : class
{
    Task<TResult> Handle(TQuery query);
}
