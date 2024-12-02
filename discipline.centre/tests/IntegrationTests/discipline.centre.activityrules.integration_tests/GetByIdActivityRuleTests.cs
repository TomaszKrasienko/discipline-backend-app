// using System.Net;
// using System.Net.Http.Json;
// using discipline.centre.activityrules.application.ActivityRules.DTOs;
// using discipline.centre.activityrules.domain;
// using discipline.centre.activityrules.domain.ValueObjects;
// using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
// using discipline.centre.activityrules.infrastructure.DAL.Documents;
// using discipline.centre.activityrules.tests.sharedkernel.Domain;
// using discipline.centre.integration_tests.shared;
// using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
// using Shouldly;
// using Xunit;
//
// namespace discipline.centre.activityrules.integration_tests;
//
// [Collection("activity-rules-module-get-activity-rule-by-id")]
// public sealed class GetByIdActivityRuleTests() : BaseTestsController("activity-rules-module")
// {
//     [Fact]
//     public async Task GetById_GivenExistingId_ShouldReturn200OkStatusCodeAndActivityRuleDto()
//     {
//         //arrange
//         var user = await AuthorizeWithFreeSubscriptionPicked();
//         var activityRule = ActivityRule.Create(ActivityRuleId.New(), user.Id, "test_title", Mode.EveryDayMode, null);
//         
//         await TestAppDb.GetCollection<ActivityRuleDocument>()
//             .InsertOneAsync(activityRule.MapAsDocument());
//         
//         //act
//         var response = await HttpClient.GetAsync($"activity-rules-module/activity-rules/{activityRule.Id.ToString()}");
//         
//         //assert
//         response.StatusCode.ShouldBe(HttpStatusCode.OK);
//
//         var result = await response.Content.ReadFromJsonAsync<ActivityRuleDto>();
//         result?.Title.ShouldBe(activityRule.Title);
//     }
//
//     [Fact]
//     public async Task GetById_GivenNotExistingId_ShouldReturn404NotFoundStatusCode()
//     {
//         //arrange
//         await AuthorizeWithFreeSubscriptionPicked();
//         
//         //act
//         var response = await HttpClient.GetAsync($"activity-rules-module/activity-rules/{ActivityRuleId.New().ToString()}");
//         
//         //assert
//         response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
//     }
//
//     [Fact]
//     public async Task GetById_Unauthorized_ShouldReturn401Unauthorized()
//     {
//         //act
//         var response = await HttpClient.GetAsync($"activity-rules-module/activity-rules/{ActivityRuleId.New().ToString()}");
//         
//         //assert
//         response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
//     }
//     
//     [Fact]
//     public async Task GetById_AuthorizedWithoutSubscription_ShouldReturn403ForbiddenStatusCode()
//     {
//         //arrange
//         await AuthorizeWithoutSubscription();
//         
//         //act
//         var response = await HttpClient.GetAsync($"activity-rules-module/activity-rules/{ActivityRuleId.New().ToString()}");
//         
//         //assert
//         response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
//     }
// }