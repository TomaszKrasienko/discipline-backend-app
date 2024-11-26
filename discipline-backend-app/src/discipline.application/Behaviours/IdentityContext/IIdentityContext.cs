using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.application.Behaviours.IdentityContext;

public interface IIdentityContext
{
    public bool IsAuthenticated { get; }
    public UserId? UserId { get; }
    public string? Status { get; }
}