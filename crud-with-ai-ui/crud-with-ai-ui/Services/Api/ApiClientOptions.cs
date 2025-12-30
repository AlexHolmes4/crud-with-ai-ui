namespace crud_with_ai_ui.Services.Api;

public sealed class ApiClientOptions
{
    public const string SectionName = "Api";

    public string BaseUrl { get; init; } = "http://localhost:5278";
    public string? ApiKey { get; init; }
}
