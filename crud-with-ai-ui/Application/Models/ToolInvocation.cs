using System.Text.Json;

namespace Application.Models;

public sealed record ToolInvocation(string Name, JsonElement Arguments);
