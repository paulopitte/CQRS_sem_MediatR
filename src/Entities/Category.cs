namespace CQRS_sem_MediatR.Entities;
public record class Category
{
    public int Id { get; set; }
    public string? Name { get; set; }

    [JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}
