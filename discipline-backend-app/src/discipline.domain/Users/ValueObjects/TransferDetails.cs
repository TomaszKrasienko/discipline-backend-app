using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.ValueObjects;

public sealed record TransferDetails
{
    public string Title { get; }
    public string CardNumber { get; }

    public TransferDetails(string title, string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new EmptyBillingTitleException();
        }
        Title = title;
        if (cardNumber.Length is <= 12 or >= 20)
        {
            throw new InvalidCardLengthException(cardNumber);
        }
        CardNumber = cardNumber;
    }
}