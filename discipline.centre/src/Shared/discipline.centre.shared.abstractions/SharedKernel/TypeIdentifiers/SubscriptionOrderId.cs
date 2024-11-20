namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record SubscriptionOrderId(Ulid Value) : ITypeId<SubscriptionOrderId>
{
    public static SubscriptionOrderId New()
        => new (Ulid.NewUlid());

    public static SubscriptionOrderId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new ArgumentException($"Can not parse stronglyTypedId of type: {nameof(SubscriptionOrderId)}");
        }

        return new SubscriptionOrderId(parsedId);
    }

    public static SubscriptionOrderId Empty()
        => new SubscriptionOrderId(Ulid.Empty);

    public override string ToString()
        => Value.ToString();
}