using System.ComponentModel.DataAnnotations;

namespace API.Requests;

public sealed class ConversationRequest
{
    [Required]
    public string Prompt { get; init; } = string.Empty;
}
