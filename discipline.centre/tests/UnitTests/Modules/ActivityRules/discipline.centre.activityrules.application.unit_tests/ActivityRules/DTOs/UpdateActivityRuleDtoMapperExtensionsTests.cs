using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.tests.sharedkernel.Application;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.DTOs;

public sealed class UpdateActivityRuleDtoMapperExtensionsTests
{
    [Fact]
    public void MapAsCommand_GivenUpdateActivityRuleDtoWithActivityRuleId_ShouldReturnUpdateActivityRuleId()
    {
        //arrange
        var dto = UpdateActivityRuleDtoFakeDataFactory.Get();
        var id = ActivityRuleId.New();
        var userId = UserId.New();
        
        //act
        var result = dto.MapAsCommand(id, userId);
        
        //assert
        result.Id.ShouldBe(id);
        result.UserId.ShouldBe(userId);
        result.Details.Title.ShouldBe(dto.Details.Title);
        result.Mode.ShouldBe(dto.Mode);
        result.SelectedDays.ShouldBeEquivalentTo(dto.SelectedDays);
    }
}