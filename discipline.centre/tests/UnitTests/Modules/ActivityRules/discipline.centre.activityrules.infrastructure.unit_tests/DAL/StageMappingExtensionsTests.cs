using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.infrastructure.unit_tests.DAL;

public sealed class StageMappingExtensionsTests
{
    [Fact]
    public void MapAsDocument_GivenStage_ShouldMapToStageDocument()
    {
        //arrange
        var stage = StageFakeDataFactory.Get(1);
        
        //act
        var document = stage.MapAsDocument();
        
        //assert
        document.StageId.ShouldBe(stage.Id.ToString());
        document.Title.ShouldBe(stage.Title.Value);
        document.Index.ShouldBe(stage.Index.Value);
    }
}