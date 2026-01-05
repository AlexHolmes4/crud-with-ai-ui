using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using crud_with_ai_ui.Models.Responses;

namespace Core.Services;

public abstract class ApiServiceBase
{
    private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    protected ApiServiceBase(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    protected async Task<ApiResponse<T>> SendAsync<T>(Func<HttpRequestMessage> requestFactory, CancellationToken cancellationToken)
    {
        using var request = requestFactory();
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var payload = await response.Content.ReadFromJsonAsync<T>(_jsonOptions, cancellationToken);
            if (payload is null)
            {
                return ApiResponse<T>.Failure(response.StatusCode, null, "Empty response payload.");
            }

            return ApiResponse<T>.Success(payload, response.StatusCode);
        }

        var problemDetails = await TryReadProblemDetailsAsync(response, cancellationToken);
        var rawError = problemDetails is null ? await response.Content.ReadAsStringAsync(cancellationToken) : null;

        return ApiResponse<T>.Failure(response.StatusCode, problemDetails, rawError);
    }

    private static async Task<ProblemDetails?> TryReadProblemDetailsAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var contentType = response.Content.Headers.ContentType?.MediaType;
        if (contentType is not null && contentType.Contains("application/problem+json", StringComparison.OrdinalIgnoreCase))
        {
            var payload = await response.Content.ReadAsStringAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(payload))
            {
                return null;
            }

            try
            {
                var validationDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(payload, _jsonOptions);
                if (validationDetails is not null && validationDetails.Errors.Count > 0)
                {
                    return validationDetails;
                }

                return JsonSerializer.Deserialize<ProblemDetails>(payload, _jsonOptions);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        return null;
    }
}
