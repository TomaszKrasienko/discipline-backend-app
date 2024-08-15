using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class EmptyBillingTitleException()
    : DisciplineException("Billing title can not be empty");