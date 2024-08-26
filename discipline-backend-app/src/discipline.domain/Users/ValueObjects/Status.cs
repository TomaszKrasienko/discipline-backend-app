using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.ValueObjects;

public sealed class Status : ValueObject
{
    private readonly List<string> _availableStatuses = ["Created", "PaidSubscriptionPicked", "FreeSubscriptionPicked"];
    public string Value { get; }

    public Status(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyStatusException();
        }

        if (!_availableStatuses.Contains(value))
        {
            throw new UnavailableStatusException(value);
        }
        
        Value = value;
    }

    public static Status Created()
        => new Status("Created");

    public static Status PaidSubscriptionPicked()
        => new Status("PaidSubscriptionPicked");

    public static Status FreeSubscriptionPicked()
        => new Status("FreeSubscriptionPicked");

    public static implicit operator string(Status status)
        => status.Value;

    public static implicit operator Status(string value)
        => new Status(value);

    public static bool operator ==(Status arg1, Status arg2)
        => arg1?.Value == arg2?.Value;

    public static bool operator !=(Status arg1, Status arg2) 
        => !(arg1 == arg2);

    public override bool Equals(object obj)
        => this == (Status)obj;

    public override int GetHashCode()
        => HashCode.Combine(_availableStatuses, Value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}