using discipline.centre.dailytrackers.domain.Rules.Stages;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.dailytrackers.domain.ValueObjects;
using discipline.centre.dailytrackers.domain.ValueObjects.Activities;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.domain;

/// <summary>
/// Represents activity of user.
/// </summary>
public sealed class Activity : Entity<ActivityId, Ulid>
{
    private HashSet<Stage>? _stages;
    
    /// <summary>
    /// Details of activity. For more, see <see cref="Details"/> 
    /// </summary>
    public Details Details { get; private set; }
    
    /// <summary>
    /// Indicates whether activity is marked as checked.
    /// </summary>
    public IsChecked IsChecked { get; private set; }
    
    /// <summary>
    /// Identifier of the parent Activity Rule.
    /// </summary>
    public ActivityRuleId? ParentActivityRuleId { get; private set; }
    
    /// <summary>
    /// Read-only collection of <see cref="Stage"/>
    /// </summary>
    public IReadOnlyCollection<Stage>? Stages => _stages?.ToArray();
    
    /// <summary>
    /// <remarks>Use only for Mongo purposes</remarks>
    /// </summary>
    public Activity(ActivityId id, Details details, IsChecked isChecked, 
        ActivityRuleId? parentActivityRuleId, HashSet<Stage>? stages) : base(id)    
    {
        Details = details;
        IsChecked = isChecked;
        ParentActivityRuleId = parentActivityRuleId;
        _stages = stages;
    }

    /// <summary>
    /// Creates a new instance of <see cref="Activity"/>.
    /// </summary>
    /// <param name="activityId">Unique identifier of activity.</param>
    /// <param name="details">Details of activity. For more <see cref="Details"/>.</param>
    /// <param name="parentActivityRuleId">Identifier of the parent Activity Rule.</param>
    /// <param name="stages">List of stages</param>
    /// <returns>New instance of <see cref="Activity"/></returns>
    internal static Activity Create(ActivityId activityId, ActivityDetailsSpecification details,
        ActivityRuleId? parentActivityRuleId, List<StageSpecification>? stages)
    {
        var activityDetails = Details.Create(details.Title, details.Note);
        var activity = new Activity(activityId, activityDetails, false, 
            parentActivityRuleId, null);
        
        if (stages is not null)
        {
            activity.AddStages(stages);
        }

        return activity;
    }

    private void AddStages(List<StageSpecification> stages)
        => stages.ForEach(AddStage);

    private void AddStage(StageSpecification stage)
    {
        if (!IsIndexValid(stage.Index))
        {
            throw new DomainException("ActivityRule.Stages.MustHaveOrderedIndex",
                "Provided stages have invalid indexes");
        }
        
        CheckStageTitleUniqueness(stage.Title);
        var newStage = Stage.Create(StageId.New(), stage.Title, stage.Index);
        _stages ??= [];
        _stages.Add(newStage);
    }

    private bool IsIndexValid(int index)
        => (_stages is null && index == 1) || _stages?.Max(x => x.Index.Value) + 1 == index;

    internal void Edit(ActivityDetailsSpecification details)
        => Details = Details.Create(details.Title, details.Note);

    internal void MarkAsChecked()
    {
        if (_stages is not null && _stages.Any())
        {
            foreach (var stage in _stages)
            {
                stage.MarkAsChecked();
            }
        }
        
        IsChecked = true;
    } 
    
    internal Stage AddStage(string title)
    {
        CheckStageTitleUniqueness(title);
        var index = _stages?.Max(x => x.Index.Value) + 1 ?? 1;
        var stage = Stage.Create(StageId.New(), title, index);
        _stages ??= [];
        _stages.Add(stage);
        return stage;
    }

    internal bool DeleteStage(StageId stageId)
    {
        var stage = _stages?.SingleOrDefault(x => x.Id == stageId);
        
        if (stage is null)
        {
            return false;
        }
        
        _stages!.Remove(stage);

        if (_stages.Count == 0)
        {
            _stages = null;
            return true;
        }
        
        var newStages = _stages!.OrderBy(x => x.Index.Value).ToList();
        for (var i = 0; i < newStages.Count; i++)
        {
            _stages!.First(x => x.Id == newStages[i].Id).ChangeIndex(i + 1);
        }

        return true;
    }

    private void CheckStageTitleUniqueness(string title)
    {
        if(_stages is not null && _stages.Any(x => x.Title == title))
        {
            throw new DomainException("ActivityRule.Stages.StageTitleMustBeUnique",
                "Stages must be unique.");
        }
    }

    internal void MarkStageAsChecked(StageId stageId)
    {
        var stage = _stages?.SingleOrDefault(x => x.Id == stageId);
        
        if (stage is null)
        {
            throw new DomainException("DailyTracker.Activity.StageNotFound",
                $"Stage with 'ID': {stageId.ToString()} for activity with 'ID': '{Id.ToString()}' was not found.");
        }
        
        stage.MarkAsChecked();

        if (_stages is not null && _stages.Any(x => !x.IsChecked.Value))
        {
            return;
        }
        
        MarkAsChecked();
    }

    public void ClearParentActivityRuleId()
        => ParentActivityRuleId = null;
}