using discipline.application.Exceptions;

namespace discipline.application.Domain.UsersCalendars.Exceptions;

public sealed class EmptyAddressException()
    : DisciplineException("Address fields can not be empty");