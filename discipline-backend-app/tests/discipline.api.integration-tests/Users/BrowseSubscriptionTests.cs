using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.DTOs;
using discipline.infrastructure.DAL.Documents.Users;
using discipline.tests.shared.Documents;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.Users;

[Collection("integration-tests")]
public sealed class BrowseSubscriptionTests : BaseTestsController
{
    [Fact]
    public async Task BrowseSubscriptions_GivenExistingSubscriptions_ShouldReturn200OkStatusCodeWithSubscriptionDtoList()
    {
        //arrange
        var subscriptionDocument = SubscriptionDocumentFactory.Get();
        await TestAppDb.GetCollection<SubscriptionDocument>().InsertOneAsync(subscriptionDocument);
        
        //act
        var result = await HttpClient.GetFromJsonAsync<List<SubscriptionDto>>("users/subscriptions");
        
        //assert
        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
    }
}