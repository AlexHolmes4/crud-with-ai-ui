using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace crud_with_ai_ui.Models.Responses;

public sealed class ApiResponse<T>
{
    public bool IsSuccess { get; init; }
    public HttpStatusCode StatusCode { get; init; }
    public T? Data { get; init; }
    public ProblemDetails? ProblemDetails { get; init; }
    public string? RawError { get; init; }

    public static ApiResponse<T> Success(T data, HttpStatusCode statusCode) => new()
    {
        IsSuccess = true,
        StatusCode = statusCode,
        Data = data
    };

    public static ApiResponse<T> Failure(HttpStatusCode statusCode, ProblemDetails? problemDetails, string? rawError) => new()
    {
        IsSuccess = false,
        StatusCode = statusCode,
        ProblemDetails = problemDetails,
        RawError = rawError
    };
}
