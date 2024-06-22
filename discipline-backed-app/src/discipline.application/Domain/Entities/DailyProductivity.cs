using discipline.application.Domain.Exceptions;
using discipline.application.Domain.ValueObjects.DailyProductivity;
using discipline.application.Domain.ValueObjects.SharedKernel;

namespace discipline.application.Domain.Entities;

internal sealed class DailyProductivity : AggregateRoot
{
    private readonly List<Activity> _activities = new();
    internal Day Day { get; private set; }
    internal IReadOnlyList<Activity> Activities => _activities;

    private DailyProductivity(Day day)
    {
        Day = day;
    }

    //For mongo
    internal DailyProductivity(Day day, List<Activity> activities)
    {
        Day = day;
        _activities = activities;
    }

    internal static DailyProductivity Create(DateTime day)
        => new DailyProductivity(day);
    
    internal void AddActivity(Guid id, string title)
    {
        if (_activities.Any(x => x.Title == title))
        {
            throw new ActivityTitleAlreadyRegisteredException(title, Day);
        }

        var activity = Activity.Create(id, title);
        _activities.Add(activity);
    }
}