using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Commands;


public sealed record DeleteActivityCommand(UserId UserId, DailyTrackerId DailyTrackerId, ActivityId ActivityId) : ICommand;

internal sealed class DeleteActivityCommandHandler(
    IReadWriteDailyTrackerRepository readWriteDailyTrackerRepository) : ICommandHandler<DeleteActivityCommand>
{
    public async Task HandleAsync(DeleteActivityCommand command, CancellationToken cancellationToken)
    {
        var dailyTracker = await readWriteDailyTrackerRepository.GetDailyTrackerByIdAsync(command.UserId, 
            command.DailyTrackerId, cancellationToken);

        if (dailyTracker is null)
        {
            return;
        }
        
        var isDeleted = dailyTracker.DeleteActivity(command.ActivityId);

        if (dailyTracker.Activities.Count == 0 && isDeleted)
        {
            await  readWriteDailyTrackerRepository.DeleteAsync(dailyTracker, cancellationToken);
        }
        else if (isDeleted)
        {
            await readWriteDailyTrackerRepository.UpdateAsync(dailyTracker, cancellationToken);
        }
    }
}

