using System.Text.Json;
using Application.Interfaces;
using Application.Models;
using Domain.Entities;

namespace Application.Services;

public sealed class AIService : IAIService
{
    private const string SystemPrompt = "You are a Claude-powered assistant that helps users perform CRUD actions for products. " +
                                        "Always select the most appropriate tool for the user request.";

    private static readonly IReadOnlyList<ToolDefinition> Tools = new List<ToolDefinition>
    {
        new(
            "get_all_products",
            "Retrieve all products.",
            "{ \"type\": \"object\", \"properties\": {}, \"required\": [] }"),
        new(
            "get_product_by_id",
            "Retrieve a product by its id.",
            "{ \"type\": \"object\", \"properties\": { \"id\": { \"type\": \"string\", \"description\": \"The product id.\" } }, \"required\": [\"id\"] }"),
        new(
            "create_product",
            "Create a new product.",
            "{ \"type\": \"object\", \"properties\": { \"name\": { \"type\": \"string\" }, \"description\": { \"type\": \"string\" }, \"price\": { \"type\": \"number\" } }, \"required\": [\"name\", \"price\"] }"),
        new(
            "delete_product",
            "Delete a product by its id.",
            "{ \"type\": \"object\", \"properties\": { \"id\": { \"type\": \"string\" } }, \"required\": [\"id\"] }")
    };

    private readonly IAnthropicMessagingService _anthropicMessagingService;
    private readonly IProductService _productService;

    public AIService(IAnthropicMessagingService anthropicMessagingService, IProductService productService)
    {
        _anthropicMessagingService = anthropicMessagingService;
        _productService = productService;
    }

    public async Task<ConversationResult> ProcessPromptAsync(string prompt, CancellationToken cancellationToken)
    {
        var toolInvocation = await _anthropicMessagingService.GetToolInvocationAsync(
            SystemPrompt,
            prompt,
            Tools,
            cancellationToken);

        if (toolInvocation is null)
        {
            return new ConversationResult(
                "I could not determine the requested action. Please rephrase your request.",
                Array.Empty<Product>(),
                null);
        }

        return toolInvocation.Name switch
        {
            "get_all_products" => await HandleGetAllAsync(cancellationToken),
            "get_product_by_id" => await HandleGetByIdAsync(toolInvocation.Arguments, cancellationToken),
            "create_product" => await HandleCreateAsync(toolInvocation.Arguments, cancellationToken),
            "delete_product" => await HandleDeleteAsync(toolInvocation.Arguments, cancellationToken),
            _ => new ConversationResult(
                "I could not determine the requested action. Please rephrase your request.",
                Array.Empty<Product>(),
                null)
        };
    }

    private async Task<ConversationResult> HandleGetAllAsync(CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllAsync(cancellationToken);
        return new ConversationResult("Retrieved all products.", products, null);
    }

    private async Task<ConversationResult> HandleGetByIdAsync(JsonElement arguments, CancellationToken cancellationToken)
    {
        if (!TryGetGuid(arguments, "id", out var id))
        {
            return new ConversationResult("A valid product id is required.", Array.Empty<Product>(), null);
        }

        var product = await _productService.GetByIdAsync(id, cancellationToken);
        return product is null
            ? new ConversationResult("No product found for that id.", Array.Empty<Product>(), null)
            : new ConversationResult("Retrieved product.", Array.Empty<Product>(), product);
    }

    private async Task<ConversationResult> HandleCreateAsync(JsonElement arguments, CancellationToken cancellationToken)
    {
        var name = arguments.TryGetProperty("name", out var nameElement) ? nameElement.GetString() : null;
        if (string.IsNullOrWhiteSpace(name))
        {
            return new ConversationResult("A product name is required.", Array.Empty<Product>(), null);
        }

        var description = arguments.TryGetProperty("description", out var descriptionElement)
            ? descriptionElement.GetString()
            : null;
        var price = arguments.TryGetProperty("price", out var priceElement) && priceElement.TryGetDecimal(out var parsedPrice)
            ? parsedPrice
            : 0m;

        var product = new Product
        {
            Name = name,
            Description = description,
            Price = price
        };

        var created = await _productService.CreateAsync(product, cancellationToken);
        return new ConversationResult("Created product.", Array.Empty<Product>(), created);
    }

    private async Task<ConversationResult> HandleDeleteAsync(JsonElement arguments, CancellationToken cancellationToken)
    {
        if (!TryGetGuid(arguments, "id", out var id))
        {
            return new ConversationResult("A valid product id is required.", Array.Empty<Product>(), null);
        }

        var deleted = await _productService.DeleteAsync(id, cancellationToken);
        return deleted
            ? new ConversationResult("Deleted product.", Array.Empty<Product>(), null)
            : new ConversationResult("No product found for that id.", Array.Empty<Product>(), null);
    }

    private static bool TryGetGuid(JsonElement arguments, string propertyName, out Guid id)
    {
        id = Guid.Empty;
        if (!arguments.TryGetProperty(propertyName, out var idElement))
        {
            return false;
        }

        var idValue = idElement.GetString();
        return Guid.TryParse(idValue, out id);
    }
}
