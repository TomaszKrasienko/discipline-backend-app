namespace discipline.centre.users.domain.Users.Repositories;

public interface IReadWriteUserRepository : IReadUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
}