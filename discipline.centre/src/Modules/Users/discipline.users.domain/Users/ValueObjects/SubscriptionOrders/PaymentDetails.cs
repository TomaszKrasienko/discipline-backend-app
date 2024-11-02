using discipline.centre.shared.abstractions.SharedKernel;
using discipline.users.domain.Users.Rules.SubscriptionOrders;

namespace discipline.users.domain.Users.ValueObjects.SubscriptionOrders;

public sealed class PaymentDetails : ValueObject
{
    private readonly string _cardNumber = null!;
    private readonly string _cvvCode = null!;
    
    public string CardNumber
    {
        get => _cardNumber;
        private init
        {
            CheckRule(new CardNumberMustBeFrom12To20LengthRule(value));
            _cardNumber = value;
        }
    }
    
    public string CvvCode
    {
        get => _cvvCode;
        private init
        {
            CheckRule(new CvvLengthMustBe3Rule(value));
            _cvvCode = value;
        }
    }

    public static PaymentDetails Create(string cardNumber, string cvvCode)
        => new (cardNumber, cvvCode);

    private PaymentDetails(string cardNumber, string cvvCode)
    {
        CardNumber = cardNumber;
        CvvCode = cvvCode;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return CardNumber;
        yield return CvvCode;
    }
}