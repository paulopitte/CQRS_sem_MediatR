namespace CQRS_sem_MediatR.Products.Commands;

public record UpdateProductCommand
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public bool Active { get; set; }
}
