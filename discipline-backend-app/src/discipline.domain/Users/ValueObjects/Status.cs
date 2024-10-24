using discipline.domain.SharedKernel;
using discipline.domain.Users.BusinessRules.Statuses;

namespace discipline.domain.Users.ValueObjects;

public sealed class Status : ValueObject
{
    public const string Created = "Created";
    public const string PaidSubscriptionPicked = "PaidSubscriptionPicked";
    public const string FreeSubscriptionPicked = "FreeSubscriptionPicked";
    
    private readonly IEnumerable<string> _availableStatuses = [
        Created, PaidSubscriptionPicked, FreeSubscriptionPicked
    ];

    private readonly string _value = null!;
    
    public string Value
    {
        get => _value;
        private init
        {
            CheckRule(new StatusCanNotBeEmptyRule(value));
            CheckRule(new StatusMustBeAvailableRule(value, _availableStatuses));
            _value = value;
        }
    }

    public Status(string value)
    {
        Value = value;
    }

    public static implicit operator string(Status status)
        => status.Value;

    public static implicit operator Status(string value)
        => new (value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}