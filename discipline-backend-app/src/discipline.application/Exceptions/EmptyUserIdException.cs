using discipline.domain.SharedKernel;

namespace discipline.application.Exceptions;

public sealed class EmptyUserIdException() 
    : DisciplineException("UserId can not be empty");