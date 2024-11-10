using System.Net;
using System.Net.Http.Json;
using discipline.centre.e2e_tests.shared;
using discipline.centre.users.application.Users.Commands;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.application.Users.Services;
using discipline.centre.users.domain.Users;
using discipline.centre.users.e2e_tests.Helpers;
using discipline.centre.users.infrastructure.DAL.Users.Documents;
using discipline.centre.users.tests.sharedkernel.Domain;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace discipline.centre.users.e2e_tests;

[Collection("users-module")]
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
        var result = await HttpClient.PostAsJsonAsync("users-module/users/sign-in", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        var response = await result.Content.ReadFromJsonAsync<TokensDto>();
        response.ShouldNotBeNull();
        response.Token.ShouldNotBeEmpty();
        response.RefreshToken.ShouldNotBeEmpty();
    }
    
    [Fact]
    public async Task SignIn_GivenNotExistingUserEmail_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var command = new SignInCommand("test@test.pl", "Test123!");
        
        //act
        var result = await HttpClient.PostAsJsonAsync("users-module/users/sign-in", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SignIn_GivenEmptyEmail_ShouldReturn422UnprocessableEntityStatusCode()
    {
        
        //arrange
        var command = new SignInCommand(string.Empty, "Test123!");
        
        //act
        var result = await HttpClient.PostAsJsonAsync("users-module/users/sign-in", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
    
    #region arrange
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IPasswordManager, TestsPasswordManager>();
        base.ConfigureServices(services);
    }
    #endregion
}