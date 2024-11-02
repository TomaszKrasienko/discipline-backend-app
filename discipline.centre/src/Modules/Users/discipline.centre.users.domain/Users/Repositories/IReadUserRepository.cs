using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.users.domain.Users.Repositories;

public interface IReadUserRepository
{
    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
    Task<bool> DoesEmailExist(string email, CancellationToken cancellationToken = default);
}