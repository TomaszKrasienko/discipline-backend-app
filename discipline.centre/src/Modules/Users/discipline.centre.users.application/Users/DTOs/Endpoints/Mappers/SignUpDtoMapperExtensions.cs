using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.Commands;

// ReSharper disable once CheckNamespace
namespace discipline.centre.users.application.Users.DTOs.Endpoints;

public static class SignUpDtoMapperExtensions
{
    public static SignUpCommand MapAsCommand(this SignUpDto request, UserId userId)
        => new SignUpCommand(userId, request.Email, request.Password, request.FirstName,
            request.LastName);
}