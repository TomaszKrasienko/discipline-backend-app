namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public record DailyTrackerId(Ulid Value) : ITypeId<DailyTrackerId, Ulid>
{
    public static DailyTrackerId Create() => Create(Ulid.NewUlid());

    public static DailyTrackerId Create(Ulid value) => new DailyTrackerId(value);

    public static DailyTrackerId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new ArgumentException($"Can not parse stronglyTypedId of type: {nameof(DailyTrackerId)}");
        }

        return new DailyTrackerId(parsedId);
    }
}