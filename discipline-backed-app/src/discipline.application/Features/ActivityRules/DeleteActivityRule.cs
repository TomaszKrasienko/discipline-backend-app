using discipline.application.Features.Base.Abstractions;
using discipline.application.Features.Configuration.Base.Abstractions;
using discipline.application.Infrastructure.DAL;

namespace discipline.application.Features.ActivityRules;

public class DeleteActivityRule
{
    
}

public sealed record DeleteActivityRuleCommand(Guid Id) : ICommand;

internal sealed class DeleteActivityRuleCommandHandler(
    DisciplineDbContext dbContext) : ICommandHandler<DeleteActivityRuleCommand>
{
    public Task HandleAsync(DeleteActivityRuleCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedE xception();
    }
}