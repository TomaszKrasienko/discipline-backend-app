using discipline.application.Behaviours;
using discipline.application.Domain.DailyProductivities.Exceptions;
using discipline.application.Domain.DailyProductivities.Repositories;
using discipline.application.Features.DailyProductivities.Configuration;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.DailyProductivities;

internal static class ChangeActivityCheck
{
    internal static WebApplication MapChangeActivityCheck(this WebApplication app)
    {
        app.MapPatch($"/{Extensions.DailyProductivityTag}/activity/{{activityId:guid}}/change-check", async (Guid activityId,
            CancellationToken cancellationToken, ICommandDispatcher commandDispatcher) =>
            {
                await commandDispatcher.HandleAsync(new ChangeActivityCheckCommand(activityId), cancellationToken);
                return Results.Ok();
            })
        .Produces(StatusCodes.Status200OK, typeof(void))
        .Produces(StatusCodes.Status400BadRequest, typeof(ErrorDto))        .Produces(StatusCodes.Status400BadRequest, typeof(ErrorDto))
        .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ErrorDto))
        .WithName(nameof(ChangeActivityCheck))
        .WithTags(Extensions.DailyProductivityTag)
        .WithOpenApi(operation => new(operation)
        {
            Description = "Changes activity check"
        });;
        return app;
    }
}

public sealed record ChangeActivityCheckCommand(Guid ActivityId) : ICommand;

public sealed class ChangeActivityCheckCommandValidator : AbstractValidator<ChangeActivityCheckCommand>
{
    public ChangeActivityCheckCommandValidator()
    {
        RuleFor(x => x.ActivityId)
            .NotEmpty()
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