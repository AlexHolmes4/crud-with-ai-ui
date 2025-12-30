using Core.Services;
using crud_with_ai_ui.Models;
using Microsoft.AspNetCore.Mvc;

namespace crud_with_ai_ui.Features.Chat.Services;

public sealed class ChatPageService
{
    private readonly ConversationsApiService _apiService;
    private readonly List<ChatMessage> _messages = new();

    public ChatPageService(ConversationsApiService apiService)
    {
        _apiService = apiService;
    }

    public IReadOnlyList<ChatMessage> Messages => _messages;
    public bool IsLoading { get; private set; }
    public ProblemDetails? Error { get; private set; }

    public async Task SendMessageAsync(string prompt, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            return;
        }

        Error = null;
        IsLoading = true;
        _messages.Add(new ChatMessage { Role = "user", Content = prompt });

        var response = await _apiService.SendMessageAsync(new ChatPromptRequest { Prompt = prompt }, cancellationToken);
        if (response.IsSuccess && response.Data is not null)
        {
            MergeMessages(response.Data.Messages);
        }
        else
        {
            Error = response.ProblemDetails ?? new ProblemDetails { Title = response.RawError ?? "Unable to send the message." };
        }

        IsLoading = false;
    }

    private void MergeMessages(IReadOnlyList<ChatMessageDto> messages)
    {
        if (messages.Count == 0)
        {
            return;
        }

        if (messages.Any(message => message.Role.Equals("user", StringComparison.OrdinalIgnoreCase)))
        {
            _messages.Clear();
        }

        foreach (var message in messages)
        {
            _messages.Add(new ChatMessage
            {
                Role = message.Role,
                Content = message.Content
            });
        }
    }
}
