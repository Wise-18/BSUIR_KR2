namespace BSUIR_KR1.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Image { get; set; }
    public string? MimeType { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
}