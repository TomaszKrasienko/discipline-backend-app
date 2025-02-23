using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.ActivityRules.Commands;

public sealed record DeleteActivityRuleCommand(UserId UserId, ActivityRuleId ActivityRuleId) : ICommand;

internal sealed class DeleteActivityRuleCommandHandler(
    IReadWriteActivityRuleRepository readWriteActivityRuleRepository) : ICommandHandler<DeleteActivityRuleCommand>
{
    public async Task HandleAsync(DeleteActivityRuleCommand command, CancellationToken cancellationToken = default)
    {
        var activityRule = await readWriteActivityRuleRepository.GetByIdAsync(command.ActivityRuleId,
            command.UserId, cancellationToken);

        if (activityRule is null)
        {
            return;
        }

        await readWriteActivityRuleRepository.DeleteAsync(activityRule, cancellationToken);
    }
}