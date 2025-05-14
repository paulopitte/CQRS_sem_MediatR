namespace CQRS_sem_MediatR.Data;
public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (!context.Categories.Any())
        {
            // Adicione o mesmo código do HasData
            context.SaveChanges();
        }
    }
}
