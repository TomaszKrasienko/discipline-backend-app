using discipline.domain.ActivityRules.Entities;
using discipline.domain.DailyProductivities.Exceptions;
using discipline.domain.DailyProductivities.ValueObjects.DailyProductivity;
using discipline.domain.SharedKernel;

namespace discipline.domain.DailyProductivities.Entities;

public sealed class DailyProductivity : AggregateRoot
{
    private readonly List<Activity> _activities = new();
    public Day Day { get; private set; }
    public EntityId UserId { get; private set; }
    public IReadOnlyList<Activity> Activities => _activities;

    private DailyProductivity(Day day, EntityId userId) : base()
    {
        Day = day;
        UserId = userId;
    }

    //For mongo
    public DailyProductivity(Day day, EntityId userId, List<Activity> activities) 
        : this(day, userId)
    {
        _activities = activities;
    }

    public static DailyProductivity Create(DateOnly day, Guid userId)
        => new DailyProductivity(day, userId);
    
    public void AddActivity(Guid id, string title)
    {
        ValidateActivity(title);
        var activity = Activity.Create(id, title);
        _activities.Add(activity);
    }

    public void AddActivityFromRule(Guid id, DateTime now, ActivityRule activityRule)
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

    public void DeleteActivity(Guid activityId)
    {
        var activity = _activities.FirstOrDefault(x => x.Id.Equals(activityId));
        if (activity is null)
        {
            throw new ActivityNotFoundException(activityId);
        }

        _activities.Remove(activity);
    }

    public void ChangeActivityCheck(Guid activityId)
    {
        var activity = _activities.FirstOrDefault(x => x.Id.Equals(activityId));
        if (activity is null)
        {
            throw new ActivityNotFoundException(activityId);
        }

        activity.ChangeCheck();
    }
}