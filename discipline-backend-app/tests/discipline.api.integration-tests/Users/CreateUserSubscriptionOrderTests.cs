using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Features.Users;
using discipline.application.Infrastructure.DAL.Documents.Users;
using discipline.domain.Users.Enums;
using discipline.tests.shared.Documents;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.Users;

[Collection("integration-tests")]
public sealed class CreateUserSubscriptionOrderTests : BaseTestsController
{
    [Fact]
    public async Task CreateUserSubscriptionOrder_GivenExistingUserAndPaidSubscription_ShouldReturn200OkStatusCodeAndAddToDb()
    {
        //arrange
        var user = await AuthorizeWithoutSubscription();
        var subscriptionDocument = SubscriptionDocumentFactory.Get(10, 100);
        await TestAppDb.GetCollection<SubscriptionDocument>().InsertOneAsync(subscriptionDocument);
        var command = new CreateUserSubscriptionOrderCommand(Guid.Empty, Guid.Empty, subscriptionDocument.Id,
            SubscriptionOrderFrequency.Monthly, new string('1', 15), "123");
        
        //act
        var result = await HttpClient.PostAsJsonAsync($"/users/create-subscription-order", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedUserDocument = await TestAppDb.GetCollection<UserDocument>().Find(x => x.Id == user.Id)
            .FirstOrDefaultAsync();
        updatedUserDocument.SubscriptionOrder.ShouldBeOfType<PaidSubscriptionOrderDocument>();
    }
    
    [Fact]
    public async Task CreateUserSubscriptionOrder_GivenExistingUserAndFreeSubscription_ShouldReturn200OkStatusCodeAndAddToDb()
    {
        //arrange
        var user = await AuthorizeWithoutSubscription();
        var subscriptionDocument = SubscriptionDocumentFactory.Get();
        await TestAppDb.GetCollection<SubscriptionDocument>().InsertOneAsync(subscriptionDocument);
        var command = new CreateUserSubscriptionOrderCommand(Guid.Empty, Guid.Empty, subscriptionDocument.Id,
            null, null, null);
        
        //act
        var result = await HttpClient.PostAsJsonAsync($"/users/create-subscription-order", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedUserDocument = await TestAppDb.GetCollection<UserDocument>().Find(x => x.Id == user.Id)
            .FirstOrDefaultAsync();
        updatedUserDocument.SubscriptionOrder.ShouldBeOfType<FreeSubscriptionOrderDocument>();
    }
    
    [Fact]
    public async Task CreateUserSubscriptionOrder_GivenNotExistingSubscription_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        var command = new CreateUserSubscriptionOrderCommand(Guid.Empty, Guid.Empty, Guid.NewGuid(),
            SubscriptionOrderFrequency.Monthly, new string('1', 14), "123");
        
        //act
        var result = await HttpClient.PostAsJsonAsync($"/users/create-subscription-order", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateUserSubscriptionOrder_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new CreateUserSubscriptionOrderCommand(Guid.Empty, Guid.Empty, Guid.NewGuid(),
            SubscriptionOrderFrequency.Monthly, new string('1', 14), "123");
        
        //act
        var result = await HttpClient.PostAsJsonAsync($"/users/create-subscription-order", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateUserSubscriptionOrder_GivenEmptySubscriptionId_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        var command = new CreateUserSubscriptionOrderCommand(Guid.Empty, Guid.Empty, Guid.Empty,
            SubscriptionOrderFrequency.Monthly, new string('1', 15), "123");
        
        //act
        var result = await HttpClient.PostAsJsonAsync($"/users/create-subscription-order", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}