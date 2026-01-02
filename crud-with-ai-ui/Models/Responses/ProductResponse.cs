namespace crud_with_ai_ui.Models.Responses;

public sealed record ProductResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public string? Sku { get; init; }
}
