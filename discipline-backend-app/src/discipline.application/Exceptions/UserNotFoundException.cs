using discipline.domain.SharedKernel;

namespace discipline.application.Exceptions;

public sealed class UserNotFoundException : DisciplineException
{
    public UserNotFoundException(string email) 
        : base($"User with \"Email\": {email} does not exists")
    {
        
    }

    public UserNotFoundException(Guid userId)
     : base($"User with \"ID\": {userId} does not exists")
    {
        
    }
}
    
    