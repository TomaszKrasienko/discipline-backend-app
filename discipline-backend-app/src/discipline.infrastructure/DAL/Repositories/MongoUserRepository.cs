using System.Linq.Expressions;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users;
using discipline.domain.Users.Repositories;
using discipline.infrastructure.DAL.Connection;
using discipline.infrastructure.DAL.Documents.Mappers;
using discipline.infrastructure.DAL.Documents.Users;
using MongoDB.Driver;

namespace discipline.infrastructure.DAL.Repositories;

internal sealed class MongoUserRepository(
    IDisciplineMongoCollection disciplineMongoCollection) : IWriteUserRepository, IReadUserRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        => await disciplineMongoCollection.GetCollection<UserDocument>()
            .InsertOneAsync(user.AsDocument(), null, cancellationToken);

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        => await disciplineMongoCollection.GetCollection<UserDocument>()
            .FindOneAndReplaceAsync(x => x.Id == user.Id.ToString(), user.AsDocument(), null, cancellationToken);

    public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
        => (await disciplineMongoCollection.GetCollection<UserDocument>()
            .Find(x => x.Id == id.Value.ToString())
            .SingleOrDefaultAsync(cancellationToken))?.AsEntity();

    public async Task<User?> GetAsync(Expression<Func<User, bool>> expression, CancellationToken cancellationToken = default)
    {
        // var documentExpression = MapExpression(expression);
        //
        // return (await disciplineMongoCollection.GetCollection<UserDocument>()
        //     .Find(documentExpression.ToFilterDefinition())
        //     .SingleOrDefaultAsync(cancellationToken)).AsEntity();
        throw new NotImplementedException();
    }

    public Task<bool> DoesEmailExist(string email, CancellationToken cancellationToken = default)
        => AnyAsync(x => x.Email == email, cancellationToken);

    public Task<bool> AnyAsync(Expression<Func<UserDocument, bool>> expression, CancellationToken cancellationToken = default)
        => disciplineMongoCollection
            .GetCollection<UserDocument>()
            .Find(expression.ToFilterDefinition())
            .AnyAsync(cancellationToken);
}

