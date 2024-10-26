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

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => (await disciplineMongoCollection.GetCollection<UserDocument>()
            .Find(x => x.Email == email).FirstOrDefaultAsync(cancellationToken))?.AsEntity();

    public Task<bool> AnyAsync(Expression<Func<User, bool>> expression, CancellationToken cancellationToken = default)
    {
        var documentExpression = MapExpression(expression);
        
        disciplineMongoCollection.GetCollection<UserDocument>()
            .Find(documentExpression.ToFilterDefinition())
            .AnyAsync(cancellationToken);
    }

    private Expression<Func<UserDocument, bool>> MapExpression(Expression<Func<User, bool>> expression)
    {
        var expressionValues = ExtractFieldAndValue(expression);
        switch (expressionValues.Name)
        {
            case nameof(User.Email) :
                return x => x.Email == (string)expressionValues.Value!;
        }
    }
    
    public static (string Name, object? Value) ExtractFieldAndValue<T>(Expression<Func<T, bool>> expression)
    {
        if (expression.Body is BinaryExpression binaryExpression)
        {
            return binaryExpression.Left is MemberExpression memberExpression &&
                   binaryExpression.Right is ConstantExpression constantExpression
                ? (memberExpression.Member.Name, constantExpression.Value)
                : throw new NotSupportedException("Only simple equality expressions are supported.");
        }

        throw new NotSupportedException("Only simple equality expressions are supported.");
    }

    public Task<bool> DoesEmailExistAsync(string email, CancellationToken cancellationToken = default)
        => disciplineMongoCollection.GetCollection<UserDocument>()
            .Find(x => x.Email == email)
            .AnyAsync(cancellationToken);
}