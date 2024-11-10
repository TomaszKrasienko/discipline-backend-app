using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.infrastructure.Validation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace discipline.centre.shared.infrastructure.Exceptions;

internal sealed class ExceptionHandler(ILogger<IExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (status, code, message) = exception switch
        {
            ValidationException exc 
                => (StatusCodes.Status422UnprocessableEntity, exc.Code, $"{exc.Message}. {exc.ValidationParams}"),
            DomainException exc 
                => (StatusCodes.Status400BadRequest, exc.Code, exc.Message),
            DisciplineException exc 
                => (StatusCodes.Status400BadRequest, exc.Code, exc.Message),
            _ => (StatusCodes.Status500InternalServerError, "Exception", "There was an error")
        };
        
        httpContext.Response.StatusCode = status;

        var problemDetails = new ProblemDetails
        {
            Status = status,
            Title = code,
            Type = exception.GetType().Name,
            Detail = message,
        };
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        logger.LogError(exception.Message);
        
        return true;
    }
}