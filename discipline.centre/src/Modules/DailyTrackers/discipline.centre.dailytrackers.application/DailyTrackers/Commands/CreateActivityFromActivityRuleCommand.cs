using discipline.centre.dailytrackers.application.ActivityRules.Clients;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.shared.abstractions.Clock;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Commands;

public sealed record CreateActivityFromActivityRuleCommand(ActivityRuleId ActivityRuleId, UserId UserId) : ICommand;

internal sealed class CreateActivityFromActivityRuleCommandHandler(
    IClock clock,
    IActivityRulesApiClient apiClient,
    IWriteReadDailyTrackerRepository repository) : ICommandHandler<CreateActivityFromActivityRuleCommand>
{
    public async Task HandleAsync(CreateActivityFromActivityRuleCommand command, CancellationToken cancellationToken = default)
    {
        var today = clock.DateNow();
        var doesExists = await repository.DoesActivityWithActivityRuleExistAsync(command.ActivityRuleId, command.UserId,
            today, cancellationToken);

        if (doesExists)
        {
            throw new AlreadyRegisteredException("CreateActivityFromActivityRuleCommand.Activity",
                $"Activity from activity rule with ID: {command.ActivityRuleId} for {today} already exists.");
        }
        
        
    }
}