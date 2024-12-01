using discipline.centre.activityrules.domain.Rules;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.activityrules.domain.ValueObjects.Stages;
using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain;

public sealed class ActivityRule : AggregateRoot<ActivityRuleId>
{
    private readonly List<Stage>? _stages;
    public UserId UserId { get; }
    public Details Details { get; private set; }
    public Mode Mode { get; private set; }
    public SelectedDays? SelectedDays { get; private set; }
    public IReadOnlyList<Stage>? Stages => _stages;
    
    /// <summary>
    /// Constructor for mapping to mongo documents
    /// </summary>
    public ActivityRule(ActivityRuleId id, UserId userId, Details details,
        Mode mode, SelectedDays? selectedDays, List<Stage>? stages) : this(id, userId, details, mode, selectedDays)
    {        
        _stages = stages;   
    }
    
    private ActivityRule(ActivityRuleId id, UserId userId, Details details,
        Mode mode, SelectedDays? selectedDays) : base(id)
    {        
        UserId = userId;
        Details = details;  
        Mode = mode;
        SelectedDays = selectedDays;
    }
    
    public static ActivityRule Create(ActivityRuleId id, UserId userId, string title, string? note, string mode, 
        List<int>? selectedDays = null, List<Stage>? stages = null)
    {
        Validate(mode, selectedDays);
        var details = Details.Create(title, note);
        var days = selectedDays is not null ? SelectedDays.Create(selectedDays) : null;
        
        return new ActivityRule(id, userId, details, mode, days);
    }

    public void Edit(string title, string? note, string mode, List<int>? selectedDays = null)
    {
        var tmp = HasChanges(title, note, mode, selectedDays); 
        if (!tmp)
        {
            throw new DomainException("ActivityRule.NoChanges",
                "Activity rule has no changes");
        }
        Validate(mode, selectedDays);
        Details = Details.Create(title, note);
        Mode = mode;
        SelectedDays = selectedDays is not null ? SelectedDays.Create(selectedDays) : null;
    }

    public bool HasChanges(string title, string? note, string? mode, List<int>? selectedDays = null)
        => (Details.HasChanges(title, note))
       || (Mode.Value != mode)
       || (SelectedDays?.HasChanges(selectedDays) ?? selectedDays is not null);

    private static void Validate(string mode, List<int>? selectedDays)
    {
        CheckRule(new ModeCannotHaveFilledSelectedDays(mode, selectedDays));
        CheckRule(new ModeMustHaveFilledSelectedDays(mode, selectedDays));   
    }
}