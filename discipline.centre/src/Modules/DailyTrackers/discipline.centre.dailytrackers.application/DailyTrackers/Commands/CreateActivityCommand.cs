using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Commands;

public sealed record CreateActivityCommand(UserId UserId, ActivityId ActivityId, DateOnly Day, ActivityDetailsSpecification Details,
    List<StageSpecification>? Stages) : ICommand;

internal sealed class CreateActivityCommandHandler(
    IWriteReadDailyTrackerRepository writeReadDailyTrackerRepository) : ICommandHandler<CreateActivityCommand>
{
    public async Task HandleAsync(CreateActivityCommand command, CancellationToken cancellationToken = default)
    {
        var dailyTracker = await writeReadDailyTrackerRepository
            .GetDailyTrackerByDayAsync(command.Day, command.UserId, cancellationToken);

        dailyTracker!.AddActivity(command.ActivityId, command.Details, null, command.Stages);

    }
}