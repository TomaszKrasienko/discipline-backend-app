using discipline.domain.SharedKernel;
using discipline.domain.Users.BusinessRules.PymentDetails;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.ValueObjects;

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

    public PaymentDetails(string cardNumber, string cvvCode)
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