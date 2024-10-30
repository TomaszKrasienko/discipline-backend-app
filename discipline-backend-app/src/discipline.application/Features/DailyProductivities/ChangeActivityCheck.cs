using discipline.application.Behaviours;
using discipline.application.Behaviours.Auth;
using discipline.application.Behaviours.CQRS;
using discipline.application.Behaviours.CQRS.Commands;
using discipline.application.Features.DailyProductivities.Configuration;
using discipline.domain.DailyProductivities.Exceptions;
using discipline.domain.DailyProductivities.Repositories;
using discipline.domain.SharedKernel.TypeIdentifiers;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace discipline.application.Features.DailyProductivities;

internal static class ChangeActivityCheck
{
    internal static WebApplication MapChangeActivityCheck(this WebApplication app)
    {
        app.MapPatch($"/{Extensions.DailyProductivityTag}/activity/{{activityId}}/change-check", async (Ulid activityId,
            CancellationToken cancellationToken, ICqrsDispatcher commandDispatcher) =>
            {
                await commandDispatcher.HandleAsync(new ChangeActivityCheckCommand( new ActivityId(activityId)), cancellationToken);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))        
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))        
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName(nameof(ChangeActivityCheck))
            .WithTags(Extensions.DailyProductivityTag)
            .WithOpenApi(operation => new(operation)
            {
                Description = "Changes activity check"
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);
        return app;
    }
}

public sealed record ChangeActivityCheckCommand(ActivityId ActivityId) : ICommand;

public sealed class ChangeActivityCheckCommandValidator : AbstractValidator<ChangeActivityCheckCommand>
{
    public ChangeActivityCheckCommandValidator()
    {
        RuleFor(x => x.ActivityId)
            .Must(id => id != new ActivityId(Ulid.Empty))
            .WithMessage("\"ActivityID\" can not be empty");
    }
}

internal sealed class ChangeActivityCheckCommandHandler(
    IDailyProductivityRepository dailyProductivityRepository) : ICommandHandler<ChangeActivityCheckCommand>
{
    public async Task HandleAsync(ChangeActivityCheckCommand command, CancellationToken cancellationToken = default)
    {
        var dailyProductivity = await dailyProductivityRepository.GetByActivityId(command.ActivityId, cancellationToken);
        if (dailyProductivity is null)
        {
            throw new ActivityNotFoundException(command.ActivityId);
        }
        
        dailyProductivity.ChangeActivityCheck(command.ActivityId);
        await dailyProductivityRepository.UpdateAsync(dailyProductivity, cancellationToken);
    }
}