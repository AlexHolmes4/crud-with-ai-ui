using System.Text.Json.Serialization;
using crud_with_ai_ui.Models.Enums;

namespace crud_with_ai_ui.Models.Responses;

public sealed record ChatMessageResponse
{
    public string Role { get; init; } = "assistant";
    public string Content { get; init; } = string.Empty;
    public ProductResponse? AffectedProduct { get; init; }

    // For API deserialization - received as string from API
    [JsonPropertyName("processedAction")]
    public string? ProcessedActionString { get; init; }

    // For UI usage - computed from string
    [JsonIgnore]
    public ProcessedActionType ProcessedAction => ParseProcessedAction(ProcessedActionString);

    private static ProcessedActionType ParseProcessedAction(string? actionString)
    {
        return actionString?.ToLowerInvariant() switch
        {
            "find_product" => ProcessedActionType.FindProduct,
            "list_products" => ProcessedActionType.ListProducts,
            "create_product" => ProcessedActionType.CreateProduct,
            "update_product" => ProcessedActionType.UpdateProduct,
            "delete_product" => ProcessedActionType.DeleteProduct,
            _ => ProcessedActionType.None
        };
    }
}
