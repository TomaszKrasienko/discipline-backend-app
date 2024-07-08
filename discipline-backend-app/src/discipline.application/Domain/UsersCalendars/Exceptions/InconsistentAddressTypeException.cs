using discipline.application.Exceptions;

namespace discipline.application.Domain.Exceptions;

public sealed class InconsistentAddressTypeException()
    : DisciplineException("Address type must be online on onside");