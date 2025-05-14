using CQRS_sem_MediatR.Products.Middlewares;
using CQRS_sem_MediatR.Products.Validations;

namespace CQRS_sem_MediatR.Products;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        var assembly = typeof(ServiceCollectionExtensions).Assembly;

        // Command Handlers
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Query Handlers
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
        .AsImplementedInterfaces()
            .WithScopedLifetime());

        /// Registrando os validadores 
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Middlewares para comandos
        services.AddScoped(typeof(IDispatcherCommandMiddleware<>), typeof(CommandLoggingMiddleware<>));
        services.AddScoped(typeof(IDispatcherCommandMiddleware<>), typeof(CommandValidationMiddleware<>));
        services.AddScoped(typeof(IDispatcherCommandMiddleware<>), typeof(TransactionMiddleware<>));

        //services.Scan(scan => scan
        //    .FromAssemblies(assembly)
        //    .AddClasses(classes => classes.AssignableTo(typeof(IDispatcherCommandMiddleware<>)))
        //    .AsImplementedInterfaces()
        //    .WithScopedLifetime());

        // Middlewares para queries
        services.AddScoped(typeof(IDispatcherQueryMiddleware<,>), typeof(QueryLoggingMiddleware<,>));
        services.AddScoped(typeof(IDispatcherQueryMiddleware<,>), typeof(QueryValidationMiddleware<,>));
        services.AddScoped(typeof(IDispatcherQueryMiddleware<,>), typeof(CachingMiddleware<,>));

        //services.Scan(scan => scan
        //    .FromAssemblies(assembly)
        //    .AddClasses(classes => classes.AssignableTo(typeof(IDispatcherQueryMiddleware<,>)))
        //    .AsImplementedInterfaces()
        //    .WithScopedLifetime());

        return services;
    }
}
