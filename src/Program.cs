using Microsoft.Extensions.Caching.Hybrid;

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

#pragma warning disable EXCS0618 // Desativa aviso de obsolescência
builder.Services.AddHybridCache(options =>
{
    options.MaximumKeyLength = 1024;
    options.MaximumPayloadBytes = 1024 * 1024 * 10; // 10 MB
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromSeconds(20),
        LocalCacheExpiration = TimeSpan.FromSeconds(20)
    };
});
#pragma warning restore EXCS0618

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
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
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Demostração do Fluxo do Padrao Mediate em Subtituição ao MediatR com Middlrewares em sua pipeline."));

    //initialize data
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(dbContext);
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
