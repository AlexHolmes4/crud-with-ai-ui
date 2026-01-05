namespace crud_with_ai_ui.Models.Enums;

/// <summary>
/// Enum representing the processed actions that Claude can perform via tool calls.
/// Maps to the API tool function names.
/// </summary>
public enum ProcessedActionType
{
    None,
    FindProduct,
    ListProducts,//todo: add this to allow just the showing of the button to nav to product list
    CreateProduct,
    UpdateProduct,
    DeleteProduct
}
