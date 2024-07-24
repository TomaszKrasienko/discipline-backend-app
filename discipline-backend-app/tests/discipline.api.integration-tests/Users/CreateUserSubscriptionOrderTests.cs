using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Domain.Users.Enums;
using discipline.application.Features.Users;
using discipline.application.Infrastructure.DAL.Documents.Users;
using discipline.tests.shared.Documents;
using discipline.tests.shared.Entities;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.Users;

[Collection("integration-tests")]
public sealed class CreateUserSubscriptionOrderTests : BaseTestsController
{
    [Fact]
    public async Task CreateUserSubscriptionOrder_GivenExistingUserAndSubscription_ShouldReturn200OkStatusCodeAndAddToDb()
    {
        //arrange
        var userDocument = UserDocumentFactory.Get();
        var subscriptionDocument = SubscriptionDocumentFactory.Get();
        await TestAppDb.GetCollection<UserDocument>().InsertOneAsync(userDocument);
        await TestAppDb.GetCollection<SubscriptionDocument>().InsertOneAsync(subscriptionDocument);
        var command = new CreateUserSubscriptionOrderCommand(Guid.Empty, Guid.Empty, subscriptionDocument.Id,
            SubscriptionOrderFrequency.Monthly, new string('1', 15), "123");
        
        //act
        var result = await HttpClient.PostAsJsonAsync($"/users/{userDocument.Id}/crate-subscription-order", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedUserDocument = await TestAppDb.GetCollection<UserDocument>().Find(x => x.Id == userDocument.Id)
            .FirstOrDefaultAsync();
    }
    
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