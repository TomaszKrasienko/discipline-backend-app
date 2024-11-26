using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Features.Users;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.infrastructure.DAL.Documents.Users;
using discipline.tests.shared.Documents;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.Users;

[Collection("integration-tests")]
public sealed class SignUpTests : BaseTestsController
{
    [Fact]
    public async Task SignUp_GivenNotExistingEmailAndValidArguments_ShouldRetrun200OkStatusCodeAndAddUser()
    {
        //arrange
        var command = new SignUpCommand(new UserId(Ulid.Empty), "test@test.pl", "Test123!",
            "test_first_name", "test_last_name");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/users/sign-up", command);
        
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
        var response = await HttpClient.PostAsJsonAsync("/users/sign-up", command);
        
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
        var response = await HttpClient.PostAsJsonAsync("/users/sign-up", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}