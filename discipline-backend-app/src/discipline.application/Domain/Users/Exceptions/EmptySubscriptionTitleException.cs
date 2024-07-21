using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class EmptySubscriptionTitleException()
    : DisciplineException("Subscription title can not be empty");
