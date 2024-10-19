using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.application.Exceptions;

public sealed class UserNotFoundException : DisciplineException
{
    public UserNotFoundException(string email) 
        : base($"User with \"Email\": {email} does not exists")
    {
        
    }

    public UserNotFoundException(UserId userId)
     : base($"User with \"ID\": {userId.ToString()} does not exists")
    {
        
    }
}
    
    