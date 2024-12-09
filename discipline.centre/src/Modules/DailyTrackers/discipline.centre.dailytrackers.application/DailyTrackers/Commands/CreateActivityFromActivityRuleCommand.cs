using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Commands;

public sealed record CreateActivityFromActivityRuleCommand(ActivityRuleId ActivityRuleId) : ICommand;

internal sealed class CreateActivityFromActivityRuleCommandHandler : ICommandHandler<CreateActivityFromActivityRuleCommand>
{
    public Task HandleAsync(CreateActivityFromActivityRuleCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}