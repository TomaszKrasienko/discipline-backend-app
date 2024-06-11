using discipline.application.Domain.ValueObjects.ActivityRule;
using discipline.application.Domain.ValueObjects.SharedKernel;

namespace discipline.application.Domain.Entities;

internal sealed class ActivityRule
{
    public EntityId Id { get; }
    public Title Title { get; private set; }
    public ActivityRule Mode { get; private set; }
    public List<DayOfWeek> SelectedDays { get; private set; }

    private ActivityRule(EntityId id)
        => Id = id;

    internal static ActivityRule Create(Guid id, string title, string mode, List<string> selectedDays)
    {
        return null;
    }
}