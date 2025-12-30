using Microsoft.Extensions.Options;

namespace Core.Services;

public sealed class ApiKeyHandler : DelegatingHandler
{
    private readonly ApiClientOptions _options;

    public ApiKeyHandler(IOptions<ApiClientOptions> options)
    {
        _options = options.Value;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(_options.ApiKey) && !request.Headers.Contains("X-API-Key"))
        {
            request.Headers.Add("X-API-Key", _options.ApiKey);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
