using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unit_tests.StageTests;

public partial class CreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnStageWithValues()
    {
        //arrange
        var id = StageId.New();
        var title = "test_stage_value";
        var index = 1;
        
        //act
        var result = Stage.Create(id, title, index);
        
        //assert
        result.Id.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
        result.Index.Value.ShouldBe(index);
    }

    [Theory]
    [MemberData(nameof(GetInvalidCreateStageData))]
    public void Create_GivenInvalidArguments_ShouldThrowDomainExceptionWithCode(StageParams @params, string code)
    {
        //act
        var exception = Record.Exception(() => Stage.Create(@params.StageId, @params.Title, @params.Index));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe(code);
    }
}