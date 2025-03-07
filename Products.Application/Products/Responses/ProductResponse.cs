namespace Products.Application.Products.Responses;

public class ProductResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public bool IsAvailable => Stock > 0;
}
