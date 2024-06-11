using System.Dynamic;
using discipline.application.Domain.ValueObjects.SharedKernel;

namespace discipline.application.Domain.Entities;

public class ActivityRule
{
    public EntityId Id { get; }
    public string Title { get; private set; }
    public string Mode { get; private set; }
    public List<string> SelectedDays { get; private set; }

    private ActivityRule(EntityId id)
        => Id = id;

    internal static ActivityRule Create(Guid id, string title, string mode, List<string> selectedDays)
    {
        return null;
    }
}