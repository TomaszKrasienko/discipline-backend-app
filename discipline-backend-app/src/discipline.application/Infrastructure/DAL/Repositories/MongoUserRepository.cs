using discipline.application.Infrastructure.DAL.Connection;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.Users;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Repositories;
using MongoDB.Driver;

namespace discipline.application.Infrastructure.DAL.Repositories;

internal sealed class MongoUserRepository(
    IDisciplineMongoCollection disciplineMongoCollection) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        => await disciplineMongoCollection.GetCollection<UserDocument>()
            .InsertOneAsync(user?.AsDocument(), null, cancellationToken);

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        => await disciplineMongoCollection.GetCollection<UserDocument>()
            .FindOneAndReplaceAsync(x => x.Id.Equals(user.Id), user.AsDocument(), null, cancellationToken);

    public async Task<User> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
        => (await disciplineMongoCollection.GetCollection<UserDocument>()
            .Find(x => x.Id == id.Value).FirstOrDefaultAsync(cancellationToken))?.AsEntity();

    public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => (await disciplineMongoCollection.GetCollection<UserDocument>()
            .Find(x => x.Email == email).FirstOrDefaultAsync(cancellationToken))?.AsEntity();

    public Task<bool> IsEmailExists(string email, CancellationToken cancellationToken = default)
        => disciplineMongoCollection.GetCollection<UserDocument>()
            .Find(x => x.Email == email)
            .AnyAsync(cancellationToken);

    public Task<bool> IsUserExists(UserId userId, CancellationToken cancellationToken = default)
        =>  disciplineMongoCollection.GetCollection<UserDocument>()
            .Find(x => x.Id == userId.Value)
            .AnyAsync(cancellationToken);
}