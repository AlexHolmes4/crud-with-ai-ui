using System.Net.Http.Json;
using crud_with_ai_ui.Models;

namespace crud_with_ai_ui.Services.Api;

public sealed class ProductsApiService : ApiServiceBase
{
    public ProductsApiService(HttpClient httpClient, ILogger<ProductsApiService> logger)
        : base(httpClient, logger)
    {
    }

    public Task<ApiResponse<IReadOnlyList<Product>>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        return SendAsync<IReadOnlyList<Product>>(
            () => new HttpRequestMessage(HttpMethod.Get, "api/v1/products"),
            cancellationToken);
    }

    public Task<ApiResponse<Product>> GetProductAsync(int id, CancellationToken cancellationToken = default)
    {
        return SendAsync<Product>(
            () => new HttpRequestMessage(HttpMethod.Get, $"api/v1/products/{id}"),
            cancellationToken);
    }
}
