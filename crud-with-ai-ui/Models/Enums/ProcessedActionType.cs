namespace crud_with_ai_ui.Models.Enums;

/// <summary>
/// Enum representing the processed actions that Claude can perform via tool calls.
/// Maps to the API tool function names.
/// </summary>
public enum ProcessedActionType
{
    None,
    FindProduct,
    ListProducts,
    CreateProduct,
    UpdateProduct,
    DeleteProduct
}
