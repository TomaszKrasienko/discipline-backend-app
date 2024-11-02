namespace discipline.centre.users.domain.Users.Repositories;

public interface IWriteUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
}