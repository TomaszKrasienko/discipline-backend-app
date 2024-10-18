namespace discipline.domain.SharedKernel.TypeIdentifiers;

public sealed record ActivityRuleId(Ulid Value) : ITypeId<ActivityRuleId>
{
    public static ActivityRuleId New()
        => new(Ulid.NewUlid());
}