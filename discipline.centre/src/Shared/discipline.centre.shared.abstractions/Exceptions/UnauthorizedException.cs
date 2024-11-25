using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.shared.abstractions.Exceptions;

public class UnauthorizedException() : DisciplineException(
    "Unauthorized", "User is unauthorized");