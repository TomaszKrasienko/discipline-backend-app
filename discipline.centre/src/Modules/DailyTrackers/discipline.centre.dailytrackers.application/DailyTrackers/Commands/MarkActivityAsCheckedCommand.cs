using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Commands;

public sealed record MarkActivityAsCheckedCommand(UserId UserId, DailyTrackerId DailyTrackerId,
    ActivityId ActivityId) : ICommand;
    
internal sealed class MarkActivityAsCheckedCommandHandler(
    IWriteReadDailyTrackerRepository writeReadDailyTrackerRepository) : ICommandHandler<MarkActivityAsCheckedCommand>
{
    public async Task HandleAsync(MarkActivityAsCheckedCommand command, CancellationToken cancellationToken = default)
    {
        var dailyTracker = await writeReadDailyTrackerRepository.GetDailyTrackerByIdAsync(command.DailyTrackerId, command.UserId,
            cancellationToken);
        
    }
}