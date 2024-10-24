using discipline.domain.SharedKernel;

namespace discipline.domain.Users.ValueObjects;

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