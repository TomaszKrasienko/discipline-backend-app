namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record UserId(Ulid Value) : ITypeId<UserId, Ulid>
{
    public static UserId New() => new(Ulid.NewUlid());

    public static UserId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new ArgumentException($"Can not parse stronglyTypedId of type: {nameof(UserId)}");
        }

        return new UserId(parsedId);
    }

    public override string ToString() => Value.ToString();
}