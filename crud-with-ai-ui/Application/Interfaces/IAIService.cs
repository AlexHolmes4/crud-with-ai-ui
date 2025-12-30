using Application.Models;

namespace Application.Interfaces;

public interface IAIService
{
    Task<ConversationResult> ProcessPromptAsync(string prompt, CancellationToken cancellationToken);
}
