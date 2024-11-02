using discipline.centre.shared.abstractions.SharedKernel;
using discipline.users.domain.Users.Rules.SubscriptionOrders;

namespace discipline.users.domain.Users.ValueObjects.SubscriptionOrders;

public sealed class PaymentDetails : ValueObject
{
    private readonly string _token = null!;

    public string Token
    {
        get => _token;
        private init
        {
            CheckRule(new PaymentTokenCanNotBeEmptyRule(value));
            _token = value;
        }
    }

    public static PaymentDetails Create(string token)
        => new (token);

    private PaymentDetails(string token)
        => Token = token;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Token;
    }
}