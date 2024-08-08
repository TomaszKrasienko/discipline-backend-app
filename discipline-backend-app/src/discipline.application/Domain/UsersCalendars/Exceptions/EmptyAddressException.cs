using discipline.application.Exceptions;
using discipline.domain.SharedKernel;

namespace discipline.application.Domain.UsersCalendars.Exceptions;

public sealed class EmptyAddressException()
    : DisciplineException("Address fields can not be empty");