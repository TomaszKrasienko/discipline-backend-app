using discipline.centre.shared.abstractions.CQRS.Queries;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.application.Users.Queries;
using discipline.centre.users.domain.Users;
using discipline.centre.users.infrastructure.DAL.Documents;
using discipline.centre.users.infrastructure.DAL.Users.Documents;
using MongoDB.Driver;

namespace discipline.centre.users.infrastructure.DAL.Users.QueryHandlers;

internal sealed class GetUserByIdQueryHandler(
    UsersMongoContext context) : IQueryHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken = default)
        => (await context.GetCollection<UserDocument>()
            .Find(x => UserId.Parse(x.Id) == query.UserId)
            .SingleOrDefaultAsync(cancellationToken))?.MapAsDto() 
           ?? throw new NotFoundException("GetUserByIdQuery.User", nameof(User), query.UserId.ToString());
}