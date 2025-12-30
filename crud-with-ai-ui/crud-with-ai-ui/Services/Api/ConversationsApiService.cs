using System.Net.Http.Json;
using crud_with_ai_ui.Models;

namespace crud_with_ai_ui.Services.Api;

public sealed class ConversationsApiService : ApiServiceBase
{
    public ConversationsApiService(HttpClient httpClient, ILogger<ConversationsApiService> logger)
        : base(httpClient, logger)
    {
    }

    public Task<ApiResponse<ChatPromptResponse>> SendMessageAsync(ChatPromptRequest request, CancellationToken cancellationToken = default)
    {
        return SendAsync<ChatPromptResponse>(
            () => new HttpRequestMessage(HttpMethod.Post, "api/v1/conversations/messages")
            {
                Content = JsonContent.Create(request)
            },
            cancellationToken);
    }
}
