using discipline.application.Domain.Users.Exceptions;

namespace discipline.application.Domain.Users.ValueObjects;

internal sealed record Status
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

    internal static Status Created()
        => new Status("Created");

    internal static Status PaidSubscriptionPicked()
        => new Status("PaidSubscriptionPicked");

    internal static Status FreeSubscriptionPicked()
        => new Status("FreeSubscriptionPicked");

    public static implicit operator string(Status status)
        => status.Value;

    public static implicit operator Status(string value)
        => new Status(value);
}