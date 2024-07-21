using discipline.application.Domain.Users.Entities;
using discipline.application.Infrastructure.DAL.Documents.Users;

namespace discipline.application.Infrastructure.DAL.Documents.Mappers;

internal static class UsersMappingExtensions
{
    internal static UserDocument AsDocument(this User entity)
        => new()
        {
            Id = entity.Id,
            Email = entity.Email,
            Password = entity.Password,
            FirstName = entity.FullName.FirstName,
            LastName = entity.FullName.LastName
        };
}