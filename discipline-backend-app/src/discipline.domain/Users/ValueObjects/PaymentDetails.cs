using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.ValueObjects;

public sealed record PaymentDetails
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
}