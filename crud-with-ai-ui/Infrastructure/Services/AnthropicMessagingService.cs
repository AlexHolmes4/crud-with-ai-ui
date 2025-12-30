using System.Text.Json;
using Application.Interfaces;
using Application.Models;
using Anthropic.SDK;
using Anthropic.SDK.Messaging;

namespace Infrastructure.Services;

public sealed class AnthropicMessagingService : IAnthropicMessagingService
{
    private readonly AnthropicClient _client;

    public AnthropicMessagingService(AnthropicClient client)
    {
        _client = client;
    }

    public async Task<ToolInvocation?> GetToolInvocationAsync(
        string systemPrompt,
        string userPrompt,
        IReadOnlyList<ToolDefinition> tools,
        CancellationToken cancellationToken)
    {
        var toolList = tools
            .Select(tool => new Tool(
                tool.Name,
                tool.Description,
                JsonDocument.Parse(tool.ParametersJson).RootElement))
            .ToList();

        var request = new MessageRequest
        {
            Model = "claude-3-5-sonnet-20240620",
            MaxTokens = 512,
            System = systemPrompt,
            Messages = new List<Message>
            {
                new(Role.User, userPrompt)
            },
            Tools = toolList
        };

        var response = await _client.Messages.CreateAsync(request, cancellationToken);
        var toolUse = response.Content.OfType<ToolUseContent>().FirstOrDefault();

        return toolUse is null
            ? null
            : new ToolInvocation(toolUse.Name, toolUse.Input);
    }
}
