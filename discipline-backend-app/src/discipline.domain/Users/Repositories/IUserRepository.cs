using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;

namespace discipline.domain.Users.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task<User> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> IsEmailExists(string email, CancellationToken cancellationToken = default);
    Task<bool> IsUserExists(UserId userId, CancellationToken cancellationToken = default);
}