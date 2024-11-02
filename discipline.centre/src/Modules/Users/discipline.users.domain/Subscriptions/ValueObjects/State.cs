using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.users.domain.Subscriptions.ValueObjects;

public sealed class State(bool isCancelled, DateOnly? activeTill) : ValueObject
{
    public bool IsCancelled { get; } = isCancelled;
    public DateOnly? ActiveTill { get; } = activeTill;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return IsCancelled;
        if (ActiveTill != null) yield return ActiveTill;
    }
}