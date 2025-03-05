using discipline.centre.dailytrackers.application.DailyTrackers.DTOs;
using discipline.centre.dailytrackers.tests.sharedkernel.Application;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.application.unit_tests.DailyTrackers.Mappers;

public sealed class CreateActivityDtoMapperExtensionsTests
{    
    [Fact]
    public void MapAsCommand_GivenCreateActivityDtoWithoutStagesAndActivityIdAndUserId_ShouldMapToCommand()
    {
        //arrange
        var createActivityDto = CreateActivityDtoFakeDataFactory.Get(false);
        var activityId = ActivityId.New();
        var userId = UserId.New();
        
        //act
        var result = createActivityDto.MapAsCommand(userId, activityId);
        
        //assert
        result.Day.ShouldBe(createActivityDto.Day);
        result.UserId.ShouldBe(userId);
        result.Details.Title.ShouldBe(createActivityDto.Details.Title);
        result.Details.Note.ShouldBe(createActivityDto.Details.Note);
        result.UserId.ShouldBe(userId);
        result.Stages.ShouldBeNull();
    }
    
    [Fact]
    public void MapAsCommand_GivenCreateActivityDtoWithStagesAndActivityIdAndUserId_ShouldMapToCommand()
    {
        //arrange
        var createActivityDto = CreateActivityDtoFakeDataFactory.Get(true);
        var activityId = ActivityId.New();
        var userId = UserId.New();
        
        //act
        var result = createActivityDto.MapAsCommand(userId, activityId);
        
        //assert
        result.Day.ShouldBe(createActivityDto.Day);
        result.UserId.ShouldBe(userId);
        result.Details.Title.ShouldBe(createActivityDto.Details.Title);
        result.Details.Note.ShouldBe(createActivityDto.Details.Note);
        result.UserId.ShouldBe(userId);
        result.Stages![0].Title.ShouldBe(createActivityDto.Stages![0].Title);
        result.Stages![0].Index.ShouldBe(createActivityDto.Stages![0].Index);
    }
}