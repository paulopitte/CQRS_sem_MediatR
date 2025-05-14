using CQRS_sem_MediatR.Products.Caching;
using CQRS_sem_MediatR.Products.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }); builder.Services.AddOpenApi();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));



// Configure a ordem EXPLÍCITA dos middlewares
builder.Services.AddScoped<IDispatcher, Dispatcher>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Habilita MemoryCache no projeto
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, MemoryCacheService>();


// registra os Handlers, Validator e Middlewares
builder.Services.AddHandlers();

var app = builder.Build();

//registrar middleware de exceção
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "api produtos"));

    //initialize data
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(dbContext);
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
