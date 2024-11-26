namespace discipline.domain.SharedKernel.TypeIdentifiers;

public sealed record EventId(Ulid Value) : ITypeId<EventId>
{
    public static EventId New()
        => new (Ulid.NewUlid());

    public override string ToString()
        => Value.ToString();
}