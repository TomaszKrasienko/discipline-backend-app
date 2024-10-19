using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.DTOs;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.Users;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.ValueObjects;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.Users;

[Collection("integration-tests")]
public sealed class MapBrowseMeTests : BaseTestsController
{
    [Fact]
    public async Task BrowseMe_GivenExistingAuthorizedUser_ShouldReturn200OkStatusCodeWithUserDto()
    {
        //arrange
        var user = UserFactory.Get();
        await TestAppDb.GetCollection<UserDocument>().InsertOneAsync(user.AsDocument());
        Authorize(user.Id, user.Status);
        
        //act
        var result = await HttpClient.GetFromJsonAsync<UserDto>("users/me");
        
        //arrange
        result.Id.ShouldBe(user.Id.Value);
    }
    
    [Fact]
    public async Task BrowseMe_GivenNotExistingUserId_ShouldReturn204NoContentStatusCode()
    {
        //arrange
        Authorize(UserId.New(), Status.FreeSubscriptionPicked());
        
        //act
        var result = await HttpClient.GetAsync("users/me");
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task BrowseMe_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var result = await HttpClient.GetAsync("users/me");
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}