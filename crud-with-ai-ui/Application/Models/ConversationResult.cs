using Domain.Entities;

namespace Application.Models;

public sealed record ConversationResult(
    string Message,
    IReadOnlyList<Product> Products,
    Product? Product);
