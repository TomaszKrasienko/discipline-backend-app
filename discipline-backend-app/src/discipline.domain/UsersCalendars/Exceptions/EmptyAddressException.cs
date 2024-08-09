using discipline.domain.SharedKernel;

namespace discipline.domain.UsersCalendars.Exceptions;

public sealed class EmptyAddressException()
    : DisciplineException("Address fields can not be empty");