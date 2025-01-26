using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Commands;

public sealed record MarkActivityStageAsCheckedCommand(UserId UserId, DailyTrackerId DailyTrackerId,
    ActivityId ActivityId, StageId StageId) : ICommand;

internal sealed class MarkActivityStageAsCheckedCommandHandler(
    IWriteReadDailyTrackerRepository writeReadDailyTrackerRepository) : ICommandHandler<MarkActivityStageAsCheckedCommand>
{
    public async Task HandleAsync(MarkActivityStageAsCheckedCommand command, CancellationToken cancellationToken = default)
    {
        var dailyTracker = await writeReadDailyTrackerRepository
            .GetDailyTrackerByIdAsync(command.DailyTrackerId, command.UserId, cancellationToken);

        if (dailyTracker is null)
        {
            throw new NotFoundException("MarkActivityStageAsChecked.DailyTracker", nameof(DailyTracker));
        }
        
        dailyTracker.MarkActivityStageAsChecked(command.ActivityId, command.StageId);
        await writeReadDailyTrackerRepository.UpdateAsync(dailyTracker, cancellationToken);
    }
}