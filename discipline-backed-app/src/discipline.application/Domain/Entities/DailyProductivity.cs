using discipline.application.Domain.ValueObjects.DailyProductivity;
using discipline.application.Domain.ValueObjects.SharedKernel;

namespace discipline.application.Domain.Entities;

internal sealed class DailyProductivity : AggregateRoot
{
    internal Day Day { get; private set; }
    internal List<Activity> ActivityItems { get; set; }

    private DailyProductivity(EntityId id, Day day)
    {
        Id = id;
        Day = day;
    }

    internal static DailyProductivity Create(Guid id, DateTime day)
        => new DailyProductivity(id, day);
}