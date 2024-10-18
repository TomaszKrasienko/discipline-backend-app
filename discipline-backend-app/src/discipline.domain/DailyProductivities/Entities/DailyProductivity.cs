using discipline.domain.ActivityRules.Entities;
using discipline.domain.DailyProductivities.Exceptions;
using discipline.domain.DailyProductivities.ValueObjects.DailyProductivity;
using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.domain.DailyProductivities.Entities;

public sealed class DailyProductivity : AggregateRoot<DailyProductivityId>
{
    private readonly List<Activity> _activities = new();
    public Day Day { get; private set; }
    public UserId UserId { get; private set; }
    public IReadOnlyList<Activity> Activities => _activities;

    private DailyProductivity(DailyProductivityId id, Day day, UserId userId) : base(id)
    {
        Day = day;
        UserId = userId;
    }

    //For mongo
    public DailyProductivity(DailyProductivityId id, Day day, UserId userId, List<Activity> activities) 
        : this(id, day, userId)
    {
        _activities = activities;
    }

    public static DailyProductivity Create(DailyProductivityId id, DateOnly day, UserId userId)
        => new DailyProductivity(id, day, userId);
    
    public void AddActivity(ActivityId id, string title)
    {
        ValidateActivity(title);
        var activity = Activity.Create(id, title);
        _activities.Add(activity);
    }

    public void AddActivityFromRule(ActivityId id, DateTime now, ActivityRule activityRule)
    {
        ValidateActivity(activityRule.Title);
        var activity = Activity.CreateFromRule(id, now, activityRule);
        if (activity is not null)
        {
            _activities.Add(activity);
        }
    }

    private void ValidateActivity(string title)
    {
        if (_activities.Any(x => x.Title == title))
        {
            throw new ActivityTitleAlreadyRegisteredException(title, Day);
        }
    }

    public void DeleteActivity(ActivityId activityId)
    {
        var activity = _activities.FirstOrDefault(x => x.Id == activityId);
        if (activity is null)
        {
            throw new ActivityNotFoundException(activityId);
        }

        _activities.Remove(activity);
    }

    public void ChangeActivityCheck(ActivityId activityId)
    {
        var activity = _activities.FirstOrDefault(x => x.Id == activityId);
        if (activity is null)
        {
            throw new ActivityNotFoundException(activityId);
        }

        activity.ChangeCheck();
    }
}