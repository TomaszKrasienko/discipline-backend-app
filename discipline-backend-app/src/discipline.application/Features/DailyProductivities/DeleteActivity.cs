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

internal static class DeleteActivity
{
    internal static WebApplication MapDeleteActivity(this WebApplication app)
    {
        app.MapDelete($"/{Extensions.DailyProductivityTag}/activity/{{activityId}}", async (Ulid activityId,
            CancellationToken cancellationToken, ICqrsDispatcher commandDispatcher) =>
            {
                await commandDispatcher.HandleAsync(new DeleteActivityCommand(new ActivityId(activityId)), cancellationToken);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .WithName(nameof(DeleteActivity))
            .WithTags(Extensions.DailyProductivityTag)
            .WithOpenApi(operation => new(operation)
            {
                Description = "Removes activity"
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);
        return app;
    }
}

public sealed record DeleteActivityCommand(ActivityId Id) : ICommand;

public sealed class DeleteActivityCommandValidator : AbstractValidator<DeleteActivityCommand>
{
    
}

internal sealed class DeleteActivityCommandHandler(
    IDailyProductivityRepository dailyProductivityRepository) : ICommandHandler<DeleteActivityCommand>
{
    public async Task HandleAsync(DeleteActivityCommand command, CancellationToken cancellationToken = default)
    {
        var dailyProductivity = await dailyProductivityRepository.GetByActivityId(command.Id, cancellationToken);
        if (dailyProductivity is null)
        {
            throw new ActivityNotFoundException(command.Id);
        }
        
        dailyProductivity.DeleteActivity(command.Id);
        await dailyProductivityRepository.UpdateAsync(dailyProductivity, cancellationToken);
    }
}