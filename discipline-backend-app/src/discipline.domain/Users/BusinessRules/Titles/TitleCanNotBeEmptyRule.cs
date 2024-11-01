using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.Titles;

internal sealed class TitleCanNotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("Subscription.Title.Empty",
        "Title can not be empty");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}