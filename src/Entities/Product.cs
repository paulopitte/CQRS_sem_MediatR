namespace CQRS_sem_MediatR.Entities;

// NAO CONSIDERO O USO DO MODELO DE ENTIDADE (DDD)
public record class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool Active { get; set; } = true;

    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
