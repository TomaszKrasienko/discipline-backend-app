using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.ValueObjects;

public sealed class PaymentDetails : ValueObject
{
    public string CardNumber { get; }
    public string CvvCode { get; }

    public PaymentDetails(string cardNumber, string cvvCode)
    {
        if (cardNumber.Length is <= 12 or >= 20)
        {
            throw new InvalidCardLengthException(cardNumber);
        }

        if (cvvCode.Length is not 3)
        {
            throw new InvalidCvvLengthException(cvvCode);
        }
        
        CardNumber = cardNumber;
        CvvCode = cvvCode;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return CardNumber;
        yield return CvvCode;
    }
}