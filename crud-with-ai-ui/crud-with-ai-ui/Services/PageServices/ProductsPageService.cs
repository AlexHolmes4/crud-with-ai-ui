using crud_with_ai_ui.Models;
using crud_with_ai_ui.Services.Api;
using Microsoft.AspNetCore.Mvc;

namespace crud_with_ai_ui.Services.PageServices;

public sealed class ProductsPageService
{
    private readonly ProductsApiService _apiService;

    public ProductsPageService(ProductsApiService apiService)
    {
        _apiService = apiService;
    }

    public IReadOnlyList<Product> Products { get; private set; } = Array.Empty<Product>();
    public bool IsLoading { get; private set; }
    public ProblemDetails? Error { get; private set; }

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        Error = null;
        IsLoading = true;

        var response = await _apiService.GetProductsAsync(cancellationToken);
        if (response.IsSuccess && response.Data is not null)
        {
            Products = response.Data;
        }
        else
        {
            Error = response.ProblemDetails ?? new ProblemDetails { Title = response.RawError ?? "Unable to load products." };
        }

        IsLoading = false;
    }
}
