using discipline.application.Domain.Users.Entities;
using discipline.application.Domain.Users.Repositories;
using discipline.application.Infrastructure.DAL.Connection;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.Users;
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

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => (await disciplineMongoCollection.GetCollection<UserDocument>()
            .Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken))?.AsEntity();

    public async Task<bool> IsEmailExists(string email, CancellationToken cancellationToken = default)
        => await disciplineMongoCollection.GetCollection<UserDocument>()
            .Find(x => x.Email == email)
            .AnyAsync(cancellationToken);
}