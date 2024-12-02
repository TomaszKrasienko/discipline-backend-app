// using discipline.centre.activityrules.infrastructure.DAL.Documents;
// using discipline.centre.activityrules.tests.sharedkernel.Infrastructure;
// using Shouldly;
// using Xunit;
//
// namespace dicipline.centre.activityrules.infrastructure.unit_tests.DAL;
//
// public sealed class ActivityRuleDocumentMappingExtensionsTests
// {
//     [Fact]
//     public void AsEntity_GivenActivityRuleDocumentWithoutSelectedDays_ShouldReturnActivityRuleWithEmptySelectedDays()
//     {
//         //arrange
//         var activityRuleDocument = ActivityRuleDocumentFakeDataFactory.Get();
//         
//         //act
//         var result = activityRuleDocument.MapAsEntity();
//         
//         //assert
//         result.Id.Value.ShouldBe(Ulid.Parse(activityRuleDocument.Id));
//         result.UserId.Value.ShouldBe(Ulid.Parse(activityRuleDocument.UserId));
//         result.Title.Value.ShouldBe(activityRuleDocument.Title);
//         result.Mode.Value.ShouldBe(activityRuleDocument.Mode);
//         result.SelectedDays.ShouldBeNull();
//     }
//     
//     [Fact]
//     public void AsEntity_GivenActivityRuleDocumentWithSelectedDays_ShouldReturnActivityRule()
//     {
//         //arrange
//         List<int> selectedDays = [0, 1, 2];
//         var activityRuleDocument = ActivityRuleDocumentFakeDataFactory.Get(selectedDays);
//         
//         //act
//         var result = activityRuleDocument.MapAsEntity();
//         
//         //assert
//         result.Id.Value.ShouldBe(Ulid.Parse(activityRuleDocument.Id));
//         result.UserId.Value.ShouldBe(Ulid.Parse(activityRuleDocument.UserId));
//         result.Title.Value.ShouldBe(activityRuleDocument.Title);
//         result.Mode.Value.ShouldBe(activityRuleDocument.Mode);
//         result.SelectedDays!.Values.Select(x => (int)x).SequenceEqual(selectedDays).ShouldBeTrue();
//     }
//     
//     [Fact]
//     public void MapAsDto_GivenActivityRuleDocumentWithoutSelectedDays_ShouldReturnActivityRuleDtoWithSelectedDaysAsNull()
//     {
//         //arrange
//         var activityRuleDocument = ActivityRuleDocumentFakeDataFactory.Get();
//         
//         //act
//         var result = activityRuleDocument.MapAsDto();
//         
//         //assert
//         result.Id.ShouldBe(Ulid.Parse(activityRuleDocument.Id));
//         result.Title.ShouldBe(activityRuleDocument.Title);
//         result.Mode.ShouldBe(activityRuleDocument.Mode);
//         result.SelectedDays.ShouldBeNull();
//     }
//     
//     [Fact]
//     public void MapAsDto_GivenActivityRuleDocumentWithSelectedDays_ShouldReturnActivityRuleDto()
//     {
//         //arrange
//         List<int> selectedDays = [1, 4];
//         var activityRuleDocument = ActivityRuleDocumentFakeDataFactory.Get(selectedDays);
//         
//         //act
//         var result = activityRuleDocument.MapAsDto();
//         
//         //assert
//         result.Id.ShouldBe(Ulid.Parse(activityRuleDocument.Id));
//         result.Title.ShouldBe(activityRuleDocument.Title);
//         result.Mode.ShouldBe(activityRuleDocument.Mode);
//         result.SelectedDays!.Contains(selectedDays[0]).ShouldBeTrue();
//         result.SelectedDays!.Contains(selectedDays[1]).ShouldBeTrue();
//     }
// }