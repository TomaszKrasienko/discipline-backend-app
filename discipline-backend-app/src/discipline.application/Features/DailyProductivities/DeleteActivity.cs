using discipline.application.Behaviours;
using discipline.application.Domain.DailyProductivities.Exceptions;
using discipline.application.Domain.DailyProductivities.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.DailyProductivities;

internal static class DeleteActivity
{
    internal static WebApplication MapDeleteActivity(this WebApplication app)
    {
        app.MapDelete("/daily-productivity/activity/{activityId:guid}", async (Guid activityId,
            CancellationToken cancellationToken, ICommandDispatcher commandDispatcher) =>
            {
                await commandDispatcher.HandleAsync(new DeleteActivityCommand(activityId), cancellationToken);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ErrorDto))
            .WithName(nameof(DeleteActivity))
            .WithOpenApi(operation => new(operation)
            {
                Description = "Removes activity"
            });
        return app;
    }
}

public sealed record DeleteActivityCommand(Guid Id) : ICommand;

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