namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record DailyProductivityId(Ulid Value) : ITypeId<DailyProductivityId>
{
    public static DailyProductivityId New()
        => new (Ulid.NewUlid());

    public static DailyProductivityId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new ArgumentException($"Can not parse stronglyTypedId of type: {nameof(DailyProductivityId)}");
        }

        return new DailyProductivityId(parsedId);
    }

    public override string ToString()
        => Value.ToString();
}