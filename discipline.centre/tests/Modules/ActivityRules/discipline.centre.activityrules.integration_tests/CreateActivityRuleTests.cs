using System.Net;
using System.Net.Http.Json;
using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.integration_tests.shared;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.integration_tests;

[Collection("activity-rules-module-create-activity-rule")]
public sealed class CreateActivityRuleTests() : BaseTestsController("activity-rules-module")
{
    [Fact]
    public async Task CreateUserSubscriptionOrder_GivenExistingUserAndPaidSubscription_ShouldReturn200OkStatusCodeAndAddToDb()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var command = new CreateActivityRuleCommand(new ActivityRuleId(Ulid.Empty), new UserId(Ulid.Empty), "Test title", Mode.EveryDayMode, null);
         
        //act
        var response = await HttpClient.PostAsJsonAsync<CreateActivityRuleCommand>("/activity-rules-module/activity-rules", command);
         
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
         
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();

        var newActivityRuleDocument = await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .Find(x => x.Id.ToString() == resourceId)
            .SingleOrDefaultAsync(); 

        newActivityRuleDocument.ShouldNotBeNull();
        newActivityRuleDocument.UserId.ShouldBe(user.Id.ToString());
    }
}