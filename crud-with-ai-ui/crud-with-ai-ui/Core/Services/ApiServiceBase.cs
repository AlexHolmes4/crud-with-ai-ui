using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using crud_with_ai_ui.Models;
using Microsoft.AspNetCore.Mvc;

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
        const int maxAttempts = 3;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
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

                if (IsTransient(response.StatusCode) && attempt < maxAttempts)
                {
                    _logger.LogWarning(
                        "Transient error {StatusCode} on {RequestUri}. Attempt {Attempt} of {MaxAttempts}.",
                        response.StatusCode,
                        request.RequestUri,
                        attempt,
                        maxAttempts);
                    await Task.Delay(TimeSpan.FromMilliseconds(200 * attempt), cancellationToken);
                    continue;
                }

                return ApiResponse<T>.Failure(response.StatusCode, problemDetails, rawError);
            }
            catch (HttpRequestException ex) when (attempt < maxAttempts)
            {
                _logger.LogWarning(ex, "Request failed on attempt {Attempt} of {MaxAttempts}.", attempt, maxAttempts);
                await Task.Delay(TimeSpan.FromMilliseconds(200 * attempt), cancellationToken);
            }
        }

        return ApiResponse<T>.Failure(HttpStatusCode.ServiceUnavailable, null, "Request failed after retries.");
    }

    private static bool IsTransient(HttpStatusCode statusCode)
        => statusCode is HttpStatusCode.RequestTimeout
            or HttpStatusCode.TooManyRequests
            or HttpStatusCode.InternalServerError
            or HttpStatusCode.BadGateway
            or HttpStatusCode.ServiceUnavailable
            or HttpStatusCode.GatewayTimeout;

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
