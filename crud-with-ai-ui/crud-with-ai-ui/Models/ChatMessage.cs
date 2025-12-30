namespace crud_with_ai_ui.Models;

public sealed record ChatMessage
{
    public string Role { get; init; } = "assistant";
    public string Content { get; init; } = string.Empty;
}
