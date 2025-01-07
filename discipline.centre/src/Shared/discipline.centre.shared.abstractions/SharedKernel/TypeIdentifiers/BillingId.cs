namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record BillingId(Ulid Value) : ITypeId<BillingId, Ulid>
{
    public static BillingId Create() => Create(Ulid.NewUlid());

    public static BillingId Create(Ulid value) => new (value);

    public static BillingId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new ArgumentException($"Can not parse stronglyTypedId of type: {nameof(BillingId)}");
        }

        return new BillingId(parsedId);
    }
}