using discipline.centre.activityrules.domain.Rules;
using discipline.centre.activityrules.domain.Rules.Stages;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.activityrules.domain.ValueObjects.Stages;
using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain;

public sealed class ActivityRule : AggregateRoot<ActivityRuleId>
{
    private List<Stage>? _stages;
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
    
    public static ActivityRule Create(ActivityRuleId id, UserId userId, ActivityRuleDetailsSpecification details, string mode, 
        List<int>? selectedDays = null, List<StageSpecification>? stages = null)
    {
        Validate(mode, selectedDays);
        var activityRuleDetails = Details.Create(details.Title, details.Note);
        var days = selectedDays is not null ? SelectedDays.Create(selectedDays) : null;
        var activityRule = new ActivityRule(id, userId, activityRuleDetails, mode, days);
        if (stages is not null)
        {
            activityRule.AddStages(stages);
        }
        
        return activityRule;
    }

    public void Edit(ActivityRuleDetailsSpecification details, string mode, List<int>? selectedDays = null)
    {
        if (!HasChanges(details, mode, selectedDays))
        {
            throw new DomainException("ActivityRule.NoChanges",
                "Activity rule has no changes");
        }
        Validate(mode, selectedDays);
        Details = Details.Create(details.Title, details.Note);
        Mode = mode;
        SelectedDays = selectedDays is not null ? SelectedDays.Create(selectedDays) : null;
    }
    
    private static void Validate(string mode, List<int>? selectedDays)
    {
        CheckRule(new ModeCannotHaveFilledSelectedDays(mode, selectedDays));
        CheckRule(new ModeMustHaveFilledSelectedDays(mode, selectedDays));   
    }
    
    public bool HasChanges(ActivityRuleDetailsSpecification details, string? mode, List<int>? selectedDays = null)
        => (Details.HasChanges(details.Title, details.Note))
       || (Mode.Value != mode)
       || (SelectedDays?.HasChanges(selectedDays) ?? selectedDays is not null);

    private void AddStages(List<StageSpecification> stages)
        => stages.ForEach(x => AddStage(x));

    public Stage AddStage(StageSpecification stage)
    {
        CheckRule(new StagesMustHaveOrderedIndexRule(_stages, stage));
        CheckRule(new StageTitleMustBeUniqueRule(_stages, stage));
        var newStage = Stage.Create(StageId.New(), stage.Title, stage.Index);
        _stages ??= [];
        _stages.Add(newStage);
        return newStage;
    }
}