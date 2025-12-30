using Application.Models;

namespace Application.Interfaces;

public interface IConversationService
{
    Task<ConversationResult> HandlePromptAsync(string prompt, CancellationToken cancellationToken);
}
