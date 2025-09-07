using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Filters;

/// <summary>
/// Global exception handler for all errors in the application
/// </summary>
public sealed class ExceptionHandler(ILogger<ExceptionHandler> _logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = CreateProblemDetails(exception);

        httpContext.Response.StatusCode = problemDetails.Status!.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private ProblemDetails CreateProblemDetails(Exception exception)
    {
        if (exception.GetType() == typeof(ValidationException))
        {
            return new ProblemDetails
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Validation error occured",
                Detail = exception.Message
            };
        }

        if (exception.GetType() == typeof(HttpRequestException))
        {
            return new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Http error occured",
                Detail = exception.Message
            };
        }

        _logger.LogError(exception, "Unknown error");
        return new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Unknown error occured"
        };
    }
}