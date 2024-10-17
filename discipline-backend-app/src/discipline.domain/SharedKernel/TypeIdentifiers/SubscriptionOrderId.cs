namespace discipline.domain.SharedKernel.TypeIdentifiers;

public sealed record SubscriptionOrderId(Ulid Value) : ITypeId
{
    public static ITypeId New()
        => new SubscriptionOrderId(Ulid.NewUlid());

    public override string ToString()
        => Value.ToString();
}