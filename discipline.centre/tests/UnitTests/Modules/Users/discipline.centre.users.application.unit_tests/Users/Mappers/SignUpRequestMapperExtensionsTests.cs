using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.DTOs.Endpoints;
using discipline.centre.users.tests.sharedkernel.Application;
using Shouldly;
using Xunit;

namespace discipline.centre.application.unit_tests.Users.Mappers;

public sealed class SignUpRequestMapperExtensionsTests
{
    [Fact]
    public void MapAsCommand_GivenSignUpRequestWithUserId_ShouldReturnSignUpCommand()
    {
        //arrange
        var userId = UserId.New();
        var request = SignUpRequestFakeDataFactory.Get();
        
        //act
        var result = request.MapAsCommand(userId);
        
        //assert
        result.Id.ShouldBe(userId);
        result.Email.ShouldBe(request.Email);
        result.Password.ShouldBe(request.Password);
        result.FirstName.ShouldBe(request.FirstName);
        result.LastName.ShouldBe(request.LastName);
    }
}