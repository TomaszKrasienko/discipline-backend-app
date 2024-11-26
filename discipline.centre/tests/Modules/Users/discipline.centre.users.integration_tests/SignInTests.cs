using System.Net;
using System.Net.Http.Json;
using discipline.centre.integration_tests.shared;
using discipline.centre.users.application.Users.Commands;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.application.Users.Services;
using discipline.centre.users.domain.Users;
using discipline.centre.users.infrastructure.DAL.Users.Documents;
using discipline.centre.users.integration_tests.Helpers;
using discipline.centre.users.tests.sharedkernel.Domain;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace discipline.centre.users.integration_tests;

[Collection("users-module-sign-in")]
public sealed class SignInTests() : BaseTestsController("users-module")
{
    [Fact]
    public async Task SignIn_GivenExistingUserWithValidPassword_ShouldReturn200OkStatusCodeWithJwtToken()
    {
        //arrange
        var user = UserFakeDataFactory.Get();
        await TestAppDb.GetCollection<UserDocument>().InsertOneAsync(user.MapAsDocument(user.Password.Value!));
        var command = new SignInCommand(user.Email, user.Password.Value!);
        
        //act
        var result = await HttpClient.PostAsJsonAsync("users-module/users/tokens", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        var response = await result.Content.ReadFromJsonAsync<TokensDto>();
        response.ShouldNotBeNull();
        response.Token.ShouldNotBeEmpty();
        response.RefreshToken.ShouldNotBeEmpty();

        var refreshTokenDto = _testRedisCache!.GetValueAsync<RefreshTokenDto>(user.Id.ToString());
        refreshTokenDto.ShouldNotBeNull();
        refreshTokenDto.Value.ShouldBe(response.RefreshToken);
    }
    
    [Fact]
    public async Task SignIn_GivenNotExistingUserEmail_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var command = new SignInCommand("test@test.pl", "Test123!");
        
        //act
        var result = await HttpClient.PostAsJsonAsync("users-module/users/tokens", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SignIn_GivenEmptyEmail_ShouldReturn422UnprocessableEntityStatusCode()
    {
        
        //arrange
        var command = new SignInCommand(string.Empty, "Test123!");
        
        //act
        var result = await HttpClient.PostAsJsonAsync("users-module/users/tokens", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
    
    #region arrange

    private TestRedisCache? _testRedisCache;
    
    protected override void ConfigureServices(IServiceCollection services)
    {
        _testRedisCache = new TestRedisCache();
        services.AddStackExchangeRedisCache(redistOptions =>
        {
            redistOptions.Configuration = _testRedisCache.ConnectionString;
        });
        services.AddSingleton<IPasswordManager, TestsPasswordManager>();
        base.ConfigureServices(services);
    }
    #endregion
}