namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record CalendarEventId(Ulid Value) : ITypeId<CalendarEventId, Ulid>
{
    public static CalendarEventId New() => new(Ulid.NewUlid());

    public static CalendarEventId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new ArgumentException($"Can not parse stronglyTypedId of type: {nameof(CalendarEventId)}");
        }

        return new CalendarEventId(parsedId);
    }

    public override string ToString() => Value.ToString();
} 