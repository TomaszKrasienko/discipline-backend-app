using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class EmptySubscriptionTitleException()
    : DisciplineException("Subscription title can not be empty");
