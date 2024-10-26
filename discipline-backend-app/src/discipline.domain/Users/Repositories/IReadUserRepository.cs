using System.Linq.Expressions;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.domain.Users.Repositories;

public interface IReadUserRepository
{
    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
    Task<User?> GetAsync(Expression<Func<User, bool>> expression, CancellationToken cancellationToken = default);
    Task<bool> DoesEmailExist(string email, CancellationToken cancellationToken = default);
}