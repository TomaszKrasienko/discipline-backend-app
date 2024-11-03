namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record EventId(Ulid Value) : ITypeId<EventId>
{
    public static EventId New()
        => new (Ulid.NewUlid());

    public static EventId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new ArgumentException($"Can not parse stronglyTypedId of type: {nameof(EventId)}");
        }

        return new EventId(parsedId);
    }

    public override string ToString()
        => Value.ToString();
}