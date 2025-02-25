using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Commands;

public sealed record DeleteActivityStageCommand(UserId UserId, DailyTrackerId DailyTrackerId, ActivityId ActivityId, 
    StageId StageId) : ICommand;
    
internal sealed class DeleteActivityStageCommandHandler(
    IWriteReadDailyTrackerRepository writeReadDailyTrackerRepository) : ICommandHandler<DeleteActivityStageCommand>
{
    public async Task HandleAsync(DeleteActivityStageCommand command, CancellationToken cancellationToken = default)
    {
        var dailyTracker = await writeReadDailyTrackerRepository.GetDailyTrackerByIdAsync(command.DailyTrackerId, 
            command.UserId, cancellationToken);

        if (dailyTracker is null)
        {
            return;
        }
        
        var isDeleted = dailyTracker.DeleteActivityStage(command.ActivityId, command.StageId);

        if (isDeleted)
        {
            await writeReadDailyTrackerRepository.UpdateAsync(dailyTracker, cancellationToken);
        }
    }
}