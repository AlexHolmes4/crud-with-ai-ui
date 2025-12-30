using API.Requests;
using API.Responses;
using Application.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/conversations")]
public sealed class ConversationsController : ControllerBase
{
    private readonly IConversationService _conversationService;

    public ConversationsController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ConversationResponse>> Create([FromBody] ConversationRequest request, CancellationToken cancellationToken)
    {
        var result = await _conversationService.HandlePromptAsync(request.Prompt, cancellationToken);
        var response = new ConversationResponse
        {
            Message = result.Message,
            Product = result.Product?.Adapt<ProductResponse>(),
            Products = result.Products.Adapt<IReadOnlyList<ProductResponse>>()
        };

        return Ok(response);
    }
}
