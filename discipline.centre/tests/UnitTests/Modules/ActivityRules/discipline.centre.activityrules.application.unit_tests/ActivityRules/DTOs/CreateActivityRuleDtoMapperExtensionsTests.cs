using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.tests.sharedkernel.Application;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.DTOs;

public sealed class CreateActivityRuleDtoMapperExtensionsTests
{
    [Fact]
    public void MapAsCommand_GivenCreateActivityRuleDtoWithActivityRuleIdAndUserId_ShouldReturnCreateActivityRuleCommand()
    {
        //arrange
        var dto = CreateActivityRuleDtoFakeDataFactory.Get();
        var activityRuleId = ActivityRuleId.New();
        var userId = UserId.New();
        
        //act
        var result = dto.MapAsCommand(activityRuleId, userId);
        
        //assert
        result.Id.ShouldBe(activityRuleId);
        result.UserId.ShouldBe(userId);
        result.Title.ShouldBe(dto.Title);
        result.Mode.ShouldBe(dto.Mode);
        result.SelectedDays.ShouldBeEquivalentTo(dto.SelectedDays);
    }
}