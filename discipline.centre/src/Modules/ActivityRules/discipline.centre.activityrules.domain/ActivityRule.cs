using discipline.centre.activityrules.domain.Rules;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain;

public sealed class ActivityRule : AggregateRoot<ActivityRuleId> 
{
    public UserId UserId { get; }
    public Title Title { get; private set; }
    public Mode Mode { get; private set; }
    public SelectedDays? SelectedDays { get; private set; }

    /// <summary>
    /// Constructor for mapping to mongo documents
    /// </summary>
    public ActivityRule(ActivityRuleId id, UserId userId, Title title,
        Mode mode, SelectedDays? selectedDays) : base(id)
    {        
        UserId = userId;
        Title = title;
        Mode = mode;
        SelectedDays = selectedDays;
    }

    public static ActivityRule Create(ActivityRuleId id, UserId userId, string title, string mode, List<int>? selectedDays = null)
    {
        Validate(mode, selectedDays);
        var days = selectedDays is not null ? SelectedDays.Create(selectedDays) : null;
        
        return new ActivityRule(id, userId, title, mode, days);
    }

    public void Edit(string title, string mode, List<int>? selectedDays = null)
    {
        var tmp = HasChanges(title, mode, selectedDays); 
        if (!tmp)
        {
            throw new DomainException("ActivityRule.NoChanges",
                "Activity rule has no changes");
        }
        Validate(mode, selectedDays);
        Title = title;
        Mode = mode;
        SelectedDays = selectedDays is not null ? SelectedDays.Create(selectedDays) : null;
    }

    public bool HasChanges(string title, string mode, List<int>? selectedDays = null)
        => (Title.Value != title)
       || (Mode.Value != mode)
       || (SelectedDays?.HasChanges(selectedDays) ?? selectedDays is not null);

    private static void Validate(string mode, List<int>? selectedDays)
    {
        CheckRule(new ModeCannotHaveFilledSelectedDays(mode, selectedDays));
        CheckRule(new ModeMustHaveFilledSelectedDays(mode, selectedDays));   
    }
}