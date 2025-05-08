var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }); builder.Services.AddOpenApi();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));


// Add repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
// Registre suas dependências
//builder.Services.AddScoped<ICommandHandler<CreateProductCommand>, CreateProductCommandHandler>();
//builder.Services.AddScoped<ICommandHandler<UpdateProductCommand>, UpdateProductCommandHandler>();
//builder.Services.AddScoped<ICommandHandler<DeleteProductCommand>, DeleteProductCommandHandler>();
//builder.Services.AddScoped<IQueryHandler<GetAllProductsQuery, List<Product>>, GetAllProductsQueryHandler>();
//builder.Services.AddScoped<IQueryHandler<GetProductByIdQuery, Product?>, GetProductByIdQueryHandler>();
//builder.Services.AddScoped<IQueryHandler<GetProductsByCategoryQuery, List<Product>>, GetProductsByCategoryQueryHandler>();

builder.Services.AddHandlers();

builder.Services.AddScoped<IDispatcher, Dispatcher>();

var app = builder.Build();

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
