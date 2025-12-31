namespace Core.Services;

public sealed class ApiClientOptions
{
    public const string SectionName = "Api";

    public string BaseUrl { get; init; } = "https://localhost:7269";
    public string? ApiKey { get; init; }
}
