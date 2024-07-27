using discipline.application.Exceptions;

namespace discipline.application.Domain.Users.Exceptions;

public sealed class BillingValueLessThanZeroException(decimal value)
    : DisciplineException($"Value: {value} is less than zero. Value can not be less than zero");