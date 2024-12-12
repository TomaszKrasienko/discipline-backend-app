using discipline.centre.dailytrackers.sharedkernel.Domain;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Mappers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.infrastructure.unit_tests.DAL.DailyTrackers;

public sealed class StageMappingExtensionsTests
{
    [Fact]
    public void MapAsDocument_GivenStage_ShouldReturnStageDocument()
    {
        //arrange
        var stage = StageFakeDataFactory.Get();
        
        //act
        var document = stage.MapAsDocument();
        
        //assert
        document.StageId.ShouldBe(stage.Id.ToString());
        document.Title.ShouldBe(stage.Title.Value);
        document.Index.ShouldBe(stage.Index.Value);
        document.IsChecked.ShouldBe(stage.IsChecked.Value);
    }
}