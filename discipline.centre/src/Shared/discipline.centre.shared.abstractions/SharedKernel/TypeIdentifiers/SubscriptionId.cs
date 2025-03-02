namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record SubscriptionId(Ulid Value) : ITypeId<SubscriptionId, Ulid>
{
    public static SubscriptionId New() => new(Ulid.NewUlid());
    
    public static SubscriptionId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new ArgumentException($"Can not parse stronglyTypedId of type: {nameof(SubscriptionId)}");
        }

        return new SubscriptionId(parsedId);
    }

    public override string ToString()
        => Value.ToString();
}