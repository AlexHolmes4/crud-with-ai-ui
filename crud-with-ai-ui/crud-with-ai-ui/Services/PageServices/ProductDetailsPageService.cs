using crud_with_ai_ui.Models;
using crud_with_ai_ui.Services.Api;
using Microsoft.AspNetCore.Mvc;

namespace crud_with_ai_ui.Services.PageServices;

public sealed class ProductDetailsPageService
{
    private readonly ProductsApiService _apiService;

    public ProductDetailsPageService(ProductsApiService apiService)
    {
        _apiService = apiService;
    }

    public Product? Product { get; private set; }
    public bool IsLoading { get; private set; }
    public ProblemDetails? Error { get; private set; }

    public async Task LoadAsync(int id, CancellationToken cancellationToken = default)
    {
        Error = null;
        IsLoading = true;

        var response = await _apiService.GetProductAsync(id, cancellationToken);
        if (response.IsSuccess && response.Data is not null)
        {
            Product = response.Data;
        }
        else
        {
            Error = response.ProblemDetails ?? new ProblemDetails { Title = response.RawError ?? "Unable to load product." };
        }

        IsLoading = false;
    }
}
