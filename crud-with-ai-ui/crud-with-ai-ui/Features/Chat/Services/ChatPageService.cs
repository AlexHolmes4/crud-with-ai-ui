using Blazored.Toast.Services;
using Core.Services;
using crud_with_ai_ui.Models.Requests;
using crud_with_ai_ui.Models.Responses;
using crud_with_ai_ui.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace crud_with_ai_ui.Features.Chat.Services;

public sealed class ChatPageService
{
    private readonly ConversationsApiService _apiService;
    private readonly IToastService? _toastService;
    private readonly List<ChatMessageResponse> _messages = new();
    private string? _conversationId;

    public ChatPageService(ConversationsApiService apiService, IToastService? toastService = null)
    {
        _apiService = apiService;
        _toastService = toastService;
    }

    public IReadOnlyList<ChatMessageResponse> Messages => _messages;
    public bool IsLoading { get; private set; }
    public bool IsShowingSpinner { get; private set; }
    public ProblemDetails? Error { get; private set; }

    public void Initialize()
    {
        const string welcomeMessage = "Hello! I'm your AI assistant. I can help you create product records. Just tell me what you'd like to do, and I'll guide you through the process.";
        _messages.Add(new ChatMessageResponse { Role = "assistant", Content = welcomeMessage });
    }

    public async Task SendMessageAsync(string prompt, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            return;
        }

        Error = null;
        IsLoading = true;
        IsShowingSpinner = true;
        _messages.Add(new ChatMessageResponse { Role = "user", Content = prompt });

        try
        {
            var response = await _apiService.SendMessageAsync(new ChatPromptRequest { Prompt = prompt, ConversationId = _conversationId }, cancellationToken);
            if (response.IsSuccess && response.Data is not null)
            {
                _conversationId = response.Data.ConversationId;
                _messages.Clear();
                _messages.AddRange(response.Data.Messages);
            }
            else
            {
                var errorMessage = response.ProblemDetails?.GetDisplayMessage() ?? response.RawError ?? "Unable to send the message.";
                Error = response.ProblemDetails ?? new ProblemDetails { Title = response.RawError ?? "Unable to send the message." };
                _toastService?.ShowError(errorMessage);
            }
        }
        catch (Exception ex)
        {
            Error = new ProblemDetails { Title = "An unexpected error occurred.", Detail = ex.Message };
            _toastService?.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
            IsShowingSpinner = false;
        }
    }

    public async Task ClearConversation()
    {
        _messages.Clear();
        _conversationId = null;
        Initialize();
    }
}
