using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.tests.sharedkernel.Application;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.DTOs;

public sealed class UpdateActivityRuleDtoMapperExtensionsTests
{
    [Fact]
    public void GivenUpdateActivityRuleDtoWithActivityRuleId_WhenMapAsCommand_ShouldReturnUpdateActivityRuleCommand()
    {
        //arrange
        var dto = UpdateActivityRuleDtoFakeDataFactory.Get();
        var activityRuleId = ActivityRuleId.New();
        var userId = UserId.New();
        
        //act
        var result = dto.MapAsCommand(userId, activityRuleId);
        
        //assert
        result.Id.ShouldBe(activityRuleId);
        result.UserId.ShouldBe(userId);
        result.Details.Title.ShouldBe(dto.Details.Title);
        result.Mode.ShouldBe(dto.Mode);
        result.SelectedDays.ShouldBeEquivalentTo(dto.SelectedDays);
    }
}