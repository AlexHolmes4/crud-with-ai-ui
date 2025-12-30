using Application.Interfaces;
using Application.Models;

namespace Application.Services;

public sealed class ConversationService : IConversationService
{
    private readonly IAIService _aiService;

    public ConversationService(IAIService aiService)
    {
        _aiService = aiService;
    }

    public Task<ConversationResult> HandlePromptAsync(string prompt, CancellationToken cancellationToken)
        => _aiService.ProcessPromptAsync(prompt, cancellationToken);
}
