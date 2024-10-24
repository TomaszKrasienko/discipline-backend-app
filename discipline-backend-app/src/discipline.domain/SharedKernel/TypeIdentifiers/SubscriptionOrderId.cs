namespace discipline.domain.SharedKernel.TypeIdentifiers;

public sealed record SubscriptionOrderId(Ulid Value) : ITypeId<SubscriptionOrderId>
{
    public static SubscriptionOrderId New()
        => new (Ulid.NewUlid());

    public override string ToString()
        => Value.ToString();
}