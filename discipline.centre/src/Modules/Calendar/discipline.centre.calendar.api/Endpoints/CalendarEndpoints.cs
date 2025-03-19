using discipline.centre.calendar.api;
using discipline.centre.calendar.application.UserCalendar.TimeEvents.DTOs.Requests;
using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.Auth;
using discipline.centre.shared.infrastructure.IdentityContext.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

internal static class CalendarEndpoints
{
    private const string CalendarTag = "calendar";
    
    internal static WebApplication MapCalendarEndpoints(WebApplication app)
    {
        app.MapPost($"api/{CalendarModule.ModuleName}/{CalendarTag}/day/{{day:dateonly}}/time-event", async (DateOnly day, 
            CreateTimeEventRequestDto dto, 
            CancellationToken cancellationToken,
            IValidator<CreateTimeEventRequestDto> validator, 
            ICqrsDispatcher cqrsDispatcher, 
            IIdentityContext identityContext) =>
        {
            var validationResult = await validator.ValidateAsync(dto, cancellationToken);

            if (!validationResult.IsValid)
            {
                 return Results.UnprocessableEntity(validationResult.Errors);
            }

            var eventId = CalendarEventId.New();
            await cqrsDispatcher.HandleAsync(dto.AsCommand(identityContext.GetUser(), day, eventId), cancellationToken);

            return Results.NoContent();
        })
        .Produces(StatusCodes.Status201Created, typeof(void))
        .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
        .Produces(StatusCodes.Status401Unauthorized, typeof(void))
        .Produces(StatusCodes.Status403Forbidden, typeof(void))
        .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
        .WithName("CreateTimeEvent")
        .WithTags(CalendarTag)
        .WithDescription("Creates time event in calendar")
        .RequireAuthorization()
        .RequireAuthorization(UserStatePolicy.Name);
        
        return app;
    }
}