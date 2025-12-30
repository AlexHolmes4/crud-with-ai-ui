namespace API.Responses;

public sealed class ConversationResponse
{
    public string Message { get; init; } = string.Empty;
    public IReadOnlyList<ProductResponse> Products { get; init; } = Array.Empty<ProductResponse>();
    public ProductResponse? Product { get; init; }
}
