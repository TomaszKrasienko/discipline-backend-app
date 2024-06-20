using discipline.application.Domain.ValueObjects.Activity;
using discipline.application.Domain.ValueObjects.SharedKernel;

namespace discipline.application.Domain.Entities;

internal sealed class Activity
{
    internal EntityId Id { get; }
    internal Title Title { get; private set; }
    internal IsChecked IsChecked { get; private set; }
    internal EntityId ParentRuleId { get; private set; }

    private Activity(EntityId id)
    {
        Id = id;
        IsChecked = false;
    }

    private Activity(EntityId id, Title title, EntityId parentRuleId) : this(id)
    {
        Title = title;
        ParentRuleId = parentRuleId;
    }

    internal static Activity Create(Guid id, string title)
    {
        var activity = new Activity(id);
        activity.ChangeTitle(title);
        return activity;
    }

    private void ChangeTitle(string value)
        => Title = value;
}