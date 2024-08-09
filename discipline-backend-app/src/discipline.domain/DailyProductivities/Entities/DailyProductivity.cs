using discipline.domain.ActivityRules.Entities;
using discipline.domain.DailyProductivities.Exceptions;
using discipline.domain.DailyProductivities.ValueObjects.DailyProductivity;
using discipline.domain.SharedKernel;

namespace discipline.domain.DailyProductivities.Entities;

internal sealed class DailyProductivity : AggregateRoot
{
    private readonly List<Activity> _activities = new();
    internal Day Day { get; private set; }
    internal EntityId UserId { get; private set; }
    internal IReadOnlyList<Activity> Activities => _activities;

    private DailyProductivity(Day day, EntityId userId) : base()
    {
        Day = day;
    }

    //For mongo
    internal DailyProductivity(Day day, EntityId userId, List<Activity> activities) 
        : this(day, userId)
    {
        _activities = activities;
    }

    internal static DailyProductivity Create(DateOnly day, Guid userId)
        => new DailyProductivity(day, userId);
    
    internal void AddActivity(Guid id, string title)
    {
        ValidateActivity(title);
        var activity = Activity.Create(id, title);
        _activities.Add(activity);
    }

    internal void AddActivityFromRule(Guid id, DateTime now, ActivityRule activityRule)
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

    internal void DeleteActivity(Guid activityId)
    {
        var activity = _activities.FirstOrDefault(x => x.Id.Equals(activityId));
        if (activity is null)
        {
            throw new ActivityNotFoundException(activityId);
        }

        _activities.Remove(activity);
    }

    internal void ChangeActivityCheck(Guid activityId)
    {
        var activity = _activities.FirstOrDefault(x => x.Id.Equals(activityId));
        if (activity is null)
        {
            throw new ActivityNotFoundException(activityId);
        }

        activity.ChangeCheck();
    }
}