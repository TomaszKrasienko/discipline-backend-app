using discipline.application.Domain.Users.Exceptions;

namespace discipline.application.Domain.Users.ValueObjects;

internal sealed record PaymentDetails
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