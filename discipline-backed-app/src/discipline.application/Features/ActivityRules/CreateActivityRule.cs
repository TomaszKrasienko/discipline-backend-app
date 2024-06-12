using discipline.application.Features.Base.Abstractions;

namespace discipline.application.Features.ActivityRules;

public static class CreateActivityRule
{
    public sealed record CreateActivityRuleCommand(Guid Id, string Title, string Mode,
        List<int> SelectedDays) : ICommand;
    
    
}