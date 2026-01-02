namespace crud_with_ai_ui.Models.Requests;

public sealed record ChatPromptRequest
{
    public string Prompt { get; init; } = string.Empty;
    public string? ConversationId { get; init; }
}
