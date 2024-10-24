using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.Titles;

internal sealed class TitleCanNotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new EmptySubscriptionTitleException();

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}