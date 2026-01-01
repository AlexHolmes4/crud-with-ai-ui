namespace crud_with_ai_ui.Models.Responses;

public sealed record ChatPromptResponse
{
    public string? ConversationId { get; init; }
    public IReadOnlyList<ChatMessageResponse> Messages { get; init; } = Array.Empty<ChatMessageResponse>();
}
