using Microsoft.AspNetCore.Mvc;

namespace crud_with_ai_ui.Models;

public static class ProblemDetailsExtensions
{
    public static string GetDisplayMessage(this ProblemDetails problemDetails)
    {
        if (problemDetails is ValidationProblemDetails validation && validation.Errors.Count > 0)
        {
            var firstError = validation.Errors.SelectMany(kvp => kvp.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(firstError))
            {
                return firstError;
            }
        }

        return problemDetails.Detail ?? problemDetails.Title ?? "An unexpected error occurred.";
    }
}
