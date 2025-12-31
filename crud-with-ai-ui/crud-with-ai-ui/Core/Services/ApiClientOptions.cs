namespace Core.Services;

public sealed class ApiClientOptions
{
    public required string BaseUrl { get; set; }
    public required string? ApiKey { get; set; }
}
