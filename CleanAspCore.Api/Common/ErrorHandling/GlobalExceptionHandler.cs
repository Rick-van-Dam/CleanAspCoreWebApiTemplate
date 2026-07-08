using System.Text.Json;
using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace CleanAspCore.Api.Common.ErrorHandling;

internal sealed partial class GlobalExceptionHandler : IExceptionHandler
{
    private static readonly string[] _missingRequiredPropertyMessage = ["Is required but missing"];

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case BadHttpRequestException { InnerException: JsonException jsonException }:
                if (jsonException.InnerException is ValidationException validationException)
                {
                    var propertyName = ExtractPropertyName(jsonException.Path);
                    var errors = validationException.Errors.Select(x => x.ErrorMessage).ToArray();
                    var problemDetails = new HttpValidationProblemDetails
                    {
                        Title = "Validation failed",
                        Errors = { [propertyName] = errors },
                        Instance = httpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                    return true;
                }

                var match = MissingJsonPropertiesRegex().Match(jsonException.Message);
                if (match.Success)
                {
                    var missingProperties = match.Groups["Missing"].Value
                        .Replace("'", "", StringComparison.Ordinal)
                        .Split(", ");
                    var missingProblemDetails = new HttpValidationProblemDetails
                    {
                        Title = "Invalid json",
                        Detail = "Required properties are missing",
                        Errors = missingProperties.ToDictionary(x => x, x => _missingRequiredPropertyMessage),
                        Instance = httpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await httpContext.Response.WriteAsJsonAsync(missingProblemDetails, cancellationToken);
                }
                else
                {
                    var jsonProblemDetails = new ProblemDetails
                    {
                        Title = "Invalid json",
                        Detail = jsonException.Message,
                        Instance = httpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await httpContext.Response.WriteAsJsonAsync(jsonProblemDetails, cancellationToken);
                }

                return true;
            default:
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return true;
        }
    }

    private static string ExtractPropertyName(string? path)
    {
        if (string.IsNullOrEmpty(path))
            return string.Empty;

        var name = path.TrimStart('$', '.');
        if (name.Length == 0)
            return name;

        return name.Length == 1
            ? char.ToUpperInvariant(name[0]).ToString()
            : char.ToUpperInvariant(name[0]) + name[1..];
    }

    [GeneratedRegex("JSON deserialization for type '.*' was missing required properties including: (?<Missing>.*)\\.")]
    private static partial Regex MissingJsonPropertiesRegex();
}
