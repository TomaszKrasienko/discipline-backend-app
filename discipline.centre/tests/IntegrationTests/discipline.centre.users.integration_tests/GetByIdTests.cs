using System.Net;
using System.Net.Http.Json;
using discipline.centre.integration_tests.shared;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.ValueObjects.Users;
using discipline.centre.users.infrastructure.DAL.Users.Documents;
using discipline.centre.users.tests.sharedkernel.Domain;
using Shouldly;
using Xunit;

namespace discipline.centre.users.integration_tests;

[Collection("users-module-get-by-id")]
public sealed class GetByIdTests() : BaseTestsController("users-module")
{
    [Fact]
    public async Task Handle_GivenExistingUserId_ShouldReturnUser()
    {
        //arrange
        var user = UserFakeDataFactory.Get();
        await TestAppDb.GetCollection<UserDocument>().InsertOneAsync(user.MapAsDocument(user.Password.Value!));
        
        Authorize(user.Id, user.Email, user.Status);
        
        //act
        var result = await HttpClient.GetAsync($"api/users-module/users/{user.Id.ToString()}");
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var payload = await result.Content.ReadFromJsonAsync<UserDto>();
        payload!.Id.ShouldBe(payload.Id);
    }
    
    [Fact]
    public async Task Handle_Unauthorized_ShouldReturn401StatusCode()
    {
        //arrange
        var user = UserFakeDataFactory.Get();
        await TestAppDb.GetCollection<UserDocument>().InsertOneAsync(user.MapAsDocument(user.Password.Value!));
        
        //act
        var result = await HttpClient.GetAsync($"api/users-module/users/{user.Id.ToString()}");
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Handle_GivenNotExistingUserId_ShouldReturn404StatusCode()
    {
        //arrange
        Authorize(UserId.New(), "test@test.pl", Status.FreeSubscriptionPicked);
        
        //act
        var result = await HttpClient.GetAsync($"api/users-module/users/{UserId.New().ToString()}");
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}