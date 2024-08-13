using discipline.domain.SharedKernel;

namespace discipline.domain.Users.Exceptions;

public sealed class BillingValueLessThanZeroException(decimal value)
    : DisciplineException($"Value: {value} is less than zero. Value can not be less than zero");