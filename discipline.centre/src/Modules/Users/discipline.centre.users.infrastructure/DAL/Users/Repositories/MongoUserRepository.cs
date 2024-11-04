using System.Linq.Expressions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.Repositories;
using discipline.centre.users.infrastructure.DAL.Documents;
using discipline.centre.users.infrastructure.DAL.Users.Documents;
using discipline.centre.users.infrastructure.Passwords;
using MongoDB.Driver;

namespace discipline.centre.users.infrastructure.DAL.Users.Repositories;

internal sealed class MongoUserRepository(
    IMongoCollectionContext disciplineMongoCollection,
    IPasswordManager passwordManager) : IWriteUserRepository, IReadUserRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        => await disciplineMongoCollection.GetCollection<UserDocument>()
            .InsertOneAsync(
                user.MapAsDocument(passwordManager.Secure(user.Password.Value!)),
            null, 
                cancellationToken);

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        await disciplineMongoCollection.GetCollection<UserDocument>()
            .FindOneAndReplaceAsync(x 
                => x.Id == user.Id.ToString(),
                user.MapAsDocument(user.Password?.Value is null ? user.Password?.HashedValue! : passwordManager.Secure(user.Password.Value)),
                null, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
        => (await disciplineMongoCollection.GetCollection<UserDocument>()
            .Find(x => x.Id == id.Value.ToString())
            .SingleOrDefaultAsync(cancellationToken))?.MapAsEntity();

    public Task<bool> DoesEmailExist(string email, CancellationToken cancellationToken = default)
        => AnyAsync(x => x.Email == email, cancellationToken);

    private Task<bool> AnyAsync(Expression<Func<UserDocument, bool>> expression, CancellationToken cancellationToken = default)
        => disciplineMongoCollection
            .GetCollection<UserDocument>()
            .Find(expression.ToFilterDefinition())
            .AnyAsync(cancellationToken);
}

