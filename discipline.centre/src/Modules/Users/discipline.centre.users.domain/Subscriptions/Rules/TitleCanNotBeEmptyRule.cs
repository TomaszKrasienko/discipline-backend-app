using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.users.domain.Subscriptions.Rules;

internal sealed class TitleCanNotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("Subscription.Title.Empty",
        "Title can not be empty");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}