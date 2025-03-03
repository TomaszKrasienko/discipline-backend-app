using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Commands;

public sealed record CheckActivityStageCommand(UserId UserId, DailyTrackerId DailyTrackerId, ActivityId ActivityId, 
    StageId StageId) : ICommand;

internal sealed class CheckActivityStageCommandHandler(
    IReadWriteDailyTrackerRepository writeReadDailyTrackerRepository) : ICommandHandler<CheckActivityStageCommand>
{
    public async Task HandleAsync(CheckActivityStageCommand command, CancellationToken cancellationToken = default)
    {
        var dailyTracker = await writeReadDailyTrackerRepository
            .GetDailyTrackerByIdAsync(command.UserId, command.DailyTrackerId, cancellationToken);

        if (dailyTracker is null)
        {
            throw new NotFoundException("CheckActivityStage.DailyTracker", nameof(DailyTracker), 
                command.DailyTrackerId.ToString());
        }
        
        dailyTracker.MarkActivityStageAsChecked(command.ActivityId, command.StageId);
        await writeReadDailyTrackerRepository.UpdateAsync(dailyTracker, cancellationToken);
    }
}