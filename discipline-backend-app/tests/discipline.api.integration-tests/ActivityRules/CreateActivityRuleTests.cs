using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.application.Features.ActivityRules;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.Users;
using discipline.tests.shared.Entities;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.api.integration_tests.ActivityRules;

[Collection("integration-tests")]
public sealed class CreateActivityRuleTests : BaseTestsController
{
     [Fact]
     public async Task Create_GivenValidArguments_ShouldReturn201CreatedStatusCode()
     {
         //arrange
         var subscription = SubscriptionFactory.Get();
         var user = UserFactory.Get();
         user.CreateFreeSubscriptionOrder(Guid.NewGuid(), subscription, DateTime.Now);
         await TestAppDb.GetCollection<UserDocument>().InsertOneAsync(user.AsDocument());
         Authorize(user.Id, user.Status);
         var command = new CreateActivityRuleCommand(Guid.Empty, Guid.Empty, "Test title", Mode.EveryDayMode(), null);
         
         //act
         var response = await HttpClient.PostAsJsonAsync<CreateActivityRuleCommand>("/activity-rules/create", command);
         
         //assert
         response.StatusCode.ShouldBe(HttpStatusCode.Created);
         
         var resourceId = GetResourceIdFromHeader(response);
         resourceId.ShouldNotBeNull();
         resourceId.ShouldNotBe(Guid.Empty);

         var newActivityRuleDocument = await TestAppDb
             .GetCollection<ActivityRuleDocument>()
             .Find(x => x.Id == resourceId)
             .SingleOrDefaultAsync();

         newActivityRuleDocument.ShouldNotBeNull();
         newActivityRuleDocument.UserId.ShouldBe(user.Id.Value);
     }
//     
//     [Fact]
//     public async Task Create_GivenAlreadyExistingTitle_ShouldReturn400BadRequestStatusCode()
//     {
//         //arrange
//         var activityRule = ActivityRuleFactory.Get();
//         await TestAppDb.GetCollection<ActivityRuleDocument>().InsertOneAsync(activityRule.AsDocument());
//         var command = new CreateActivityRuleCommand(Guid.Empty, activityRule.Title, Mode.EveryDayMode(), null);
//         
//         //act
//         var response = await HttpClient.PostAsJsonAsync<CreateActivityRuleCommand>("/activity-rules/create", command);
//         
//         //assert
//         response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
//     }
//     
//     [Fact]
//     public async Task Create_GivenInvalidRequest_ShouldReturn422UnprocessableEntityStatusCode()
//     {
//         //arrange
//         var command = new CreateActivityRuleCommand(Guid.Empty, string.Empty, Mode.EveryDayMode(), null);
//         
//         //act
//         var response = await HttpClient.PostAsJsonAsync<CreateActivityRuleCommand>("/activity-rules/create", command);
//         
//         //assert
//         response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
//     }
}
