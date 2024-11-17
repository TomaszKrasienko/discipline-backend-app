using System.Net;
using System.Net.Http.Json;
using discipline.centre.e2e_tests.shared;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.Commands;
using discipline.centre.users.infrastructure.DAL.Users.Documents;
using discipline.centre.users.tests.sharedkernel.Infrastructure;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.users.integration_tests;

[Collection("users-module-sign-up")]
public sealed class SignUpTests() : BaseTestsController("users-module")
{
    [Fact]
    public async Task SignUp_GivenNotRegisteredEmailAndValidArguments_ShouldReturn200OkStatusCodeAndAddUserToDb()
    {
        //arrange
        var command = new SignUpCommand(new UserId(Ulid.Empty), "test@test.pl", "Test123!",
            "test_first_name", "test_last_name");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("users-module/users", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var isUserExists = await TestAppDb
            .GetCollection<UserDocument>()
            .Find(x => x.Email == command.Email)
            .AnyAsync();

        isUserExists.ShouldBeTrue();
    }
    
    [Fact]
    public async Task SignUp_GivenAlreadyExistingEmail_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var userDocument = UserDocumentFactory.Get();
        await TestAppDb
            .GetCollection<UserDocument>()
            .InsertOneAsync(userDocument);
        
        var command = new SignUpCommand(new UserId(Ulid.Empty), userDocument.Email, "Test132!", "test_first_name",
            "test_last_name");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("users-module/users", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SignUp_GivenTooWeakPassword_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        var command = new SignUpCommand(new UserId(Ulid.Empty), "test@test.pl", "Test132", "test_first_name",
            "test_last_name");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("users-module/users", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}