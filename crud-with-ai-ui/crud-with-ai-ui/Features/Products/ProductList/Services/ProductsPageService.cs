using Core.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using crud_with_ai_ui.Models.Responses;

namespace crud_with_ai_ui.Features.Products.ProductList.Services;

public sealed class ProductsPageService
{
    private readonly ProductsApiService _apiService;
    private readonly NavigationManager _navigation;
    public ProductsPageService(ProductsApiService apiService, NavigationManager navigation)
    {
        _apiService = apiService;
        _navigation = navigation;
    }

    public IReadOnlyList<ProductResponse> Products { get; private set; } = Array.Empty<ProductResponse>();
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

    public async Task NavigateToProductAsync(int productId, CancellationToken cancellationToken = default)
    {
        _navigation.NavigateTo($"/product/{productId}");
    }
}
