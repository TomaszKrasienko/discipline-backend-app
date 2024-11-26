using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.users.application.Users.Exceptions;

internal sealed class InvalidPasswordException() 
    : DisciplineException("SignIn.Password", "Password is invalid");