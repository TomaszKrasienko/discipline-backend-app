using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Behaviours;
using discipline.application.Behaviours.Passwords;
using discipline.application.DTOs;
using discipline.application.Features.Users;
using discipline.infrastructure.DAL.Connection;
using discipline.infrastructure.DAL.Documents.Mappers;
using discipline.infrastructure.DAL.Documents.Users;
using discipline.tests.shared.Entities;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.Users;

[Collection("integration-tests")]
public sealed class RefreshUserTokenTests : BaseTestsController
{
    [Fact]
    public async Task RefreshUserToken_GivenValidRefreshToken_ShouldReturn200OkStatusCodeWithNewRefreshTokenAndNewToken()
    {
        //arrange
        var user = UserFactory.Get();
        await TestAppDb.GetCollection<UserDocument>().InsertOneAsync(user.AsDocument());
        var signInCommand = new SignInCommand(user.Email, user.Password);
        var signInResult = await HttpClient.PostAsJsonAsync("/users/sign-in", signInCommand);
        var signInTokens = await signInResult.Content.ReadFromJsonAsync<TokensDto>();
        await Task.Delay(TimeSpan.FromSeconds(1));
        var command = new RefreshTokenCommand(signInTokens.RefreshToken);

        //act
        var result = await HttpClient.PostAsJsonAsync("users/refresh-token", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        var response = await result.Content.ReadFromJsonAsync<TokensDto>();
        response.Token.ShouldNotBeEmpty();
        response.Token.ShouldNotBe(signInTokens.Token);
        response.RefreshToken.ShouldNotBeEmpty();
        response.RefreshToken.ShouldNotBe(signInTokens.RefreshToken);
    }

    [Fact]
    public async Task RefreshUserToken_GivenNotExistingValidRefreshToken_ShouldReturn401StatusCode()
    {
       //arrange
       var command = new RefreshTokenCommand(Guid.NewGuid().ToString());
       
       //act
       var result = await HttpClient.PostAsJsonAsync("users/refresh-token", command);
        
       //assert
       result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task RefreshUserToken_GivenEmptyRefreshToken_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        var command = new RefreshTokenCommand(string.Empty);
       
        //act
        var result = await HttpClient.PostAsJsonAsync("users/refresh-token", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
    
    #region arrange    
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IPasswordManager, TestsPasswordManager>();
        services.AddSingleton<IMongoCollectionNameConvention, TestsMongoCollectionNameConvention>();
    }
    #endregion
}