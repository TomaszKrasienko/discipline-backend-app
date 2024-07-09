using discipline.application.Domain.DailyProductivities.Entities;
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
        
        //act
        var result = DailyProductivity.Create(DateOnly.FromDateTime(day));
        
        //assert
        result.Day.Value.ShouldBe(DateOnly.FromDateTime(day));
    } 
}