using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.activityrules.tests.sharedkernel.Infrastructure;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.infrastructure.unit_tests.DAL;

public sealed class StageDocumentMappingExtensionsTests
{
    [Fact]
    public void MapAsEntity_GivenStageDocument_ShouldMapToEntity()
    {
        //arrange
        var stage = StageDocumentFakeDataFactory.Get();
        
        //act
        var result = stage.MapAsEntity();
        
        //assert
        result.Id.ShouldBe(StageId.Parse(stage.StageId));
        result.Title.Value.ShouldBe(stage.Title);
        result.Index.Value.ShouldBe(stage.Index);
    }
    
    [Fact]
    public void MapAsDto_GivenStageDocument_ShouldMapToDto()
    {
        //arrange
        var stage = StageDocumentFakeDataFactory.Get();
        
        //act
        var result = stage.MapAsDto();
        
        //assert
        result.StageId.ShouldBe(StageId.Parse(stage.StageId));
        result.Title.ShouldBe(stage.Title);
        result.Index.ShouldBe(stage.Index);
    }
}