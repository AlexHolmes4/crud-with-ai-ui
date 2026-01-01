namespace crud_with_ai_ui.Models;

public sealed record ChatPromptRequest
{
    public string Prompt { get; init; } = string.Empty;
    public string? ConversationId { get; init; }
}

public sealed record ChatMessageDto
{
    public string Role { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
}

public sealed record ChatPromptResponse
{
    public string? ConversationId { get; init; }
    public IReadOnlyList<ChatMessageDto> Messages { get; init; } = Array.Empty<ChatMessageDto>();
}
