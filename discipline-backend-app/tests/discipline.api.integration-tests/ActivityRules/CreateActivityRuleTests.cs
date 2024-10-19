using System.Net;
using System.Net.Http.Json;
using discipline.api.integration_tests._Helpers;
using discipline.application.Features.ActivityRules;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.Users;
using discipline.domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;
using discipline.domain.Users.ValueObjects;
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
         var user = await AuthorizeWithFreeSubscriptionPicked();
         var command = new CreateActivityRuleCommand(new ActivityRuleId(Ulid.Empty), new UserId(Ulid.Empty), "Test title", Mode.EveryDayMode(), null);
         
         //act
         var response = await HttpClient.PostAsJsonAsync<CreateActivityRuleCommand>("/activity-rules/create", command);
         
         //assert
         response.StatusCode.ShouldBe(HttpStatusCode.Created);
         
         var resourceId = GetResourceIdFromHeader(response);
         resourceId.ShouldNotBeNull();

         var newActivityRuleDocument = await TestAppDb
             .GetCollection<ActivityRuleDocument>()
             .Find(x => x.Id.ToString() == resourceId)
             .SingleOrDefaultAsync(); 

         newActivityRuleDocument.ShouldNotBeNull();
         newActivityRuleDocument.UserId.ShouldBe(user.Id.Value);
     }
     
     [Fact]
     public async Task Create_GivenAlreadyExistingTitle_ShouldReturn400BadRequestStatusCode()
     {
         //arrange
         await AuthorizeWithFreeSubscriptionPicked();
         var activityRule = ActivityRuleFactory.Get();
         await TestAppDb.GetCollection<ActivityRuleDocument>().InsertOneAsync(activityRule.AsDocument());
         var command = new CreateActivityRuleCommand(new ActivityRuleId(Ulid.Empty), new UserId(Ulid.Empty),activityRule.Title, Mode.EveryDayMode(), null);
         
         //act
         var response = await HttpClient.PostAsJsonAsync<CreateActivityRuleCommand>("/activity-rules/create", command);
         
         //assert
         response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
     }
     
     [Fact]
     public async Task Create_GivenInvalidRequest_ShouldReturn422UnprocessableEntityStatusCode()
     {
         //arrange
         await AuthorizeWithFreeSubscriptionPicked();
         var command = new CreateActivityRuleCommand(new ActivityRuleId(Ulid.Empty), new UserId(Ulid.Empty),
             string.Empty, Mode.EveryDayMode(), null);
         
         //act
         var response = await HttpClient.PostAsJsonAsync<CreateActivityRuleCommand>("/activity-rules/create", command);
         
         //assert
         response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
     }

     [Fact]
     public async Task Create_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
     {
         //arrange
         var command = new CreateActivityRuleCommand(new ActivityRuleId(Ulid.Empty), new UserId(Ulid.Empty),
             "test_title", Mode.EveryDayMode(), null);
        
         //act
         var response = await HttpClient.PostAsJsonAsync("activity-rules/create", command);
        
         //assert
         response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
     }
     
     [Fact]
     public async Task Create_AuthorizedByUserWithStatusCreated_ShouldReturn403ForbiddenStatusCode()
     {
         //arrange
         await AuthorizeWithoutSubscription();
         var command = new CreateActivityRuleCommand(new ActivityRuleId(Ulid.Empty), new UserId(Ulid.Empty), 
             "test_title", Mode.EveryDayMode(), null);
        
         //act
         var response = await HttpClient.PostAsJsonAsync("activity-rules/create", command);
        
         //assert
         response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
     }
}
