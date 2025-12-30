namespace Core.Services;

public sealed class ApiClientOptions
{
    public const string SectionName = "Api";

    public string BaseUrl { get; init; } = "http://localhost:5278";
    public string? ApiKey { get; init; }
}
