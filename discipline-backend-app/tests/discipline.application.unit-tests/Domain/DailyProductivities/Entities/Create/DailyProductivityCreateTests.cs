using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.DailyProductivities.Entities.Create;

public sealed class DailyProductivityCreateTests
{
    [Fact]
    public void Create_GivenWithArguments_ShouldReturnDailyProductivityWithFilledFields()
    {
        //arrange
        var day = DateTime.Now;
        var userId = Guid.NewGuid();
        
        //act
        var result = DailyProductivity.Create(DateOnly.FromDateTime(day), userId);
        
        //assert
        result.Day.Value.ShouldBe(DateOnly.FromDateTime(day));
        result.UserId.Value.ShouldBe(userId);
    } 
}