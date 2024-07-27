using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class EmptyBillingTitleException()
    : DisciplineException("Billing title can not be empty");