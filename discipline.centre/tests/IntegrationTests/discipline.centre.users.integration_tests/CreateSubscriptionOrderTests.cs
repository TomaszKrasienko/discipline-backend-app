using System.Net;
using System.Net.Http.Json;
using discipline.centre.integration_tests.shared;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.Commands;
using discipline.centre.users.domain.Users.Enums;
using discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;
using discipline.centre.users.infrastructure.DAL.Users.Documents;
using discipline.centre.users.tests.sharedkernel.Infrastructure;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.users.integration_tests;

[Collection("users-module-create-subscription-order")]
public sealed class CreateSubscriptionOrderTests() : BaseTestsController("users-module")
{
    [Fact]
    public async Task CreateUserSubscriptionOrder_GivenExistingUserAndPaidSubscription_ShouldReturn200OkStatusCodeAndAddToDb()
    {
        //arrange
        var user = await AuthorizeWithoutSubscription();
        var subscriptionDocument = SubscriptionDocumentFactory.Get(10, 100);
        await TestAppDb.GetCollection<SubscriptionDocument>().InsertOneAsync(subscriptionDocument);
        var command = new CreateUserSubscriptionOrderCommand(new UserId(Ulid.Empty), new SubscriptionOrderId(Ulid.Empty), new SubscriptionId(Ulid.Parse((string)subscriptionDocument.Id)),
            SubscriptionOrderFrequency.Monthly, Guid.NewGuid().ToString());
        
        //act
        var result = await HttpClient.PostAsJsonAsync($"users-module/users/subscription-order", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedUserDocument = await TestAppDb.GetCollection<UserDocument>().Find(x => x.Id == user.Id.ToString())
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
        var command = new CreateUserSubscriptionOrderCommand(UserId.Empty(), SubscriptionOrderId.Empty(), 
            SubscriptionId.Parse(subscriptionDocument.Id), null, Guid.NewGuid().ToString());
        
        //act
        var result = await HttpClient.PostAsJsonAsync($"users-module/users/subscription-order", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedUserDocument = await TestAppDb.GetCollection<UserDocument>().Find(x => x.Id == user.Id.ToString())
            .FirstOrDefaultAsync();
        updatedUserDocument.SubscriptionOrder.ShouldBeOfType<FreeSubscriptionOrderDocument>();
    }
    
    [Fact]
    public async Task CreateUserSubscriptionOrder_GivenNotExistingSubscription_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        var command = new CreateUserSubscriptionOrderCommand(new UserId(Ulid.Empty), new SubscriptionOrderId(Ulid.Empty),
            SubscriptionId.New(), SubscriptionOrderFrequency.Monthly, Guid.NewGuid().ToString());
        
        //act
        var result = await HttpClient.PostAsJsonAsync($"users-module/users/subscription-order", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateUserSubscriptionOrder_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new CreateUserSubscriptionOrderCommand(new UserId(Ulid.Empty), new SubscriptionOrderId(Ulid.Empty),
            SubscriptionId.New(), SubscriptionOrderFrequency.Monthly, Guid.NewGuid().ToString());
        
        //act
        var result = await HttpClient.PostAsJsonAsync($"users-module/users/subscription-order", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateUserSubscriptionOrder_GivenEmptySubscriptionId_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        var command = new CreateUserSubscriptionOrderCommand(new UserId(Ulid.Empty), new SubscriptionOrderId(Ulid.Empty),
            new SubscriptionId(Ulid.Empty), SubscriptionOrderFrequency.Monthly, Guid.NewGuid().ToString());
        
        //act
        var result = await HttpClient.PostAsJsonAsync($"users-module/users/subscription-order", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}