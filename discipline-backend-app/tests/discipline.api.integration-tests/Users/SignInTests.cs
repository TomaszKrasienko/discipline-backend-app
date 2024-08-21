using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Behaviours;
using discipline.application.DTOs;
using discipline.application.Features.Users;
using discipline.application.Infrastructure.DAL.Connection;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.Users;
using discipline.tests.shared.Entities;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.Users;

[Collection("integration-tests")]
public sealed class SignInTests : BaseTestsController
{
    [Fact]
    public async Task SignIn_GivenExistingUserWithValidPassword_ShouldRetrun200OkStatusCodeWithJwtToken()
    {
        //arrange
        var user = UserFactory.Get();
        await TestAppDb.GetCollection<UserDocument>().InsertOneAsync(user.AsDocument());
        var command = new SignInCommand(user.Email, user.Password);
        
        //act
        var result = await HttpClient.PostAsJsonAsync("/users/sign-in", command);
        
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
        var result = await HttpClient.PostAsJsonAsync("/users/sign-in", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SignIn_GivenEmptyEmail_ShouldReturn422UnprocessableEntityStatusCode()
    {
        
        //arrange
        var command = new SignInCommand(string.Empty, "Test123!");
        
        //act
        var result = await HttpClient.PostAsJsonAsync("/users/sign-in", command);
        
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