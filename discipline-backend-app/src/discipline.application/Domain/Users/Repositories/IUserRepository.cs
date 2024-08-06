using discipline.application.Domain.Users.Entities;

namespace discipline.application.Domain.Users.Repositories;

internal interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> IsEmailExists(string email, CancellationToken cancellationToken = default);
    Task<bool> IsUserExists(Guid userId, CancellationToken cancellationToken = default);
}