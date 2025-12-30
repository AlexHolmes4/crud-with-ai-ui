using Application.Models;

namespace Application.Interfaces;

public interface IAnthropicMessagingService
{
    Task<ToolInvocation?> GetToolInvocationAsync(
        string systemPrompt,
        string userPrompt,
        IReadOnlyList<ToolDefinition> tools,
        CancellationToken cancellationToken);
}
