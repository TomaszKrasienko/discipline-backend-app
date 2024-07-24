using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Domain.Users.Enums;
using discipline.application.Features.Users;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.Users;

[Collection("integration-tests")]
public sealed class CreateUserSubscriptionOrderTests : BaseTestsController
{
    [Fact]
    public async Task CreateUserSubscriptionOrder_GivenNotExistingUser_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var command = new CreateUserSubscriptionOrderCommand(Guid.Empty, Guid.Empty, Guid.NewGuid(),
            SubscriptionOrderFrequency.Monthly, new string('1', 14), "123");
        
        //act
        var result = await HttpClient.PostAsJsonAsync($"/users/{Guid.NewGuid()}/crate-subscription-order", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateUserSubscriptionOrder_GivenTooShortCardNumber_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        var command = new CreateUserSubscriptionOrderCommand(Guid.Empty, Guid.Empty, Guid.NewGuid(),
            SubscriptionOrderFrequency.Monthly, new string('1', 10), "123");
        
        //act
        var result = await HttpClient.PostAsJsonAsync($"/users/{Guid.NewGuid()}/crate-subscription-order", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}