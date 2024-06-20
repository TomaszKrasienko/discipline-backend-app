using discipline.application.Domain.Entities;
using discipline.application.Domain.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Entities.DailyProductivities;

public sealed class ActivityCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnActivityWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var title = "Activity title";
        
        //act
        var result = Activity.Create(id, title);
        
        //assert
        result.Id.Value.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
    }
    
    [Fact]
    public void Create_GivenEmptyTitle_ShouldThrowEmptyActivityTitleException()
    {
        //act
        var exception = Record.Exception(() => Activity.Create(Guid.NewGuid(), string.Empty));
        
        //assert
        exception.ShouldBeOfType<EmptyActivityTitleException>();
    }
}