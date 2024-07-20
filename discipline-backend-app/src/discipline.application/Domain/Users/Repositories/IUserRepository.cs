using discipline.application.Domain.Users.Entities;

namespace discipline.application.Domain.Users.Repositories;

internal interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> IsEmailExists(string email, CancellationToken cancellationToken = default);
}