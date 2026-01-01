using crud_with_ai_ui.Models.Responses;

namespace Core.Services;

public sealed class ProductsApiService : ApiServiceBase
{
    public ProductsApiService(HttpClient httpClient, ILogger<ProductsApiService> logger)
        : base(httpClient, logger)
    {
    }

    public Task<ApiResponse<IReadOnlyList<ProductResponse>>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        return SendAsync<IReadOnlyList<ProductResponse>>(
            () => new HttpRequestMessage(HttpMethod.Get, "api/v1/products"),
            cancellationToken);
    }

    public Task<ApiResponse<ProductResponse>> GetProductAsync(int id, CancellationToken cancellationToken = default)
    {
        return SendAsync<ProductResponse>(
            () => new HttpRequestMessage(HttpMethod.Get, $"api/v1/products/{id}"),
            cancellationToken);
    }
}
