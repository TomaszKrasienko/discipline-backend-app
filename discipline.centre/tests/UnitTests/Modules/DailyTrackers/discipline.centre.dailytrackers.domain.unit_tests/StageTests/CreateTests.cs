using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.StageTests;

public partial class CreateTests
{
    [Fact]
    public void Stage_GivenValidArguments_ShouldReturnStageWithExpectedValuesAndIsCheckedAsFalse()
    {
        //arrange
        var stageId = StageId.New();
        var title = "test_stage_title";
        var index = 1;
        
        //act
        var result = Stage.Create(stageId, title, index);
        
        //assert
        result.Id.ShouldBe(stageId);
        result.Title.Value.ShouldBe(title);
        result.Index.Value.ShouldBe(index);
        result.IsChecked.Value.ShouldBeFalse();
    }

    [Theory]
    [MemberData(nameof(GetInvalidCreateData))]
    public void GivenInvalidArgument_ShouldThrowDomainExceptionWithCode(CreateTestParameters parameters, string code)
    {
        //act
        var exception = Record.Exception(() => Stage.Create(parameters.StageId, parameters.Title, parameters.Index));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe(code);
    }
}