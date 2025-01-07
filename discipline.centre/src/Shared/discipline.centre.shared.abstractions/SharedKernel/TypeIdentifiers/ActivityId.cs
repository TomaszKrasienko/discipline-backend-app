namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record ActivityId(Ulid Value) : ITypeId<ActivityId, Ulid>
{
    public static ActivityId Create() => Create(Ulid.NewUlid());
    
    public static ActivityId Create(Ulid value) => new (value);

    public static ActivityId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new ArgumentException($"Can not parse stronglyTypedId of type: {nameof(ActivityId)}");
        }

        return new ActivityId(parsedId);
    }
}