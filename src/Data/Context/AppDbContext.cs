using Microsoft.EntityFrameworkCore;
namespace CQRS_sem_MediatR.Data.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Entities.Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);

        // Seed de categorias (antes dos produtos)
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Eletrônicos" },
            new Category { Id = 2, Name = "Acessórios" }
        );

        // Seed de produtos
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Smartphone Premium",
                Price = 3999.99m,
                Stock = 50,
                CategoryId = 1,
                Active = true
            },
            new Product
            {
                Id = 2,
                Name = "Notebook Ultrafino",
                Price = 5999.99m,
                Stock = 30,
                CategoryId = 1,
                Active = true
            },
            new Product
            {
                Id = 3,
                Name = "Fones de ouvido",
                Price = 17.99m,
                Stock = 200,
                CategoryId = 2,
                Active = true
            },
            new Product
            {
                Id = 4,
                Name = "Pad Mouse",
                Price = 8.99m,
                Stock = 150,
                CategoryId = 2,
                Active = true
            },
            new Product
            {
                Id = 5,
                Name = "Fone de Ouvido Bluetooth",
                Price = 299.99m,
                Stock = 80,
                CategoryId = 1,
                Active = true
            }
        );
    }
}
