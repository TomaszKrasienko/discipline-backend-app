namespace discipline.domain.SharedKernel.TypeIdentifiers;

public sealed record SubscriptionId(Ulid Value) : ITypeId<SubscriptionId>
{
    public static SubscriptionId New() 
        => new(Ulid.NewUlid());

    public override string ToString()
        => Value.ToString();
}