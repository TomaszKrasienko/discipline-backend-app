using discipline.domain.UsersCalendars.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.UsersCalendars.Entities.Create;

public sealed class UserCalendarCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnUserCalendar()
    {
        //arrange
        var day = new DateOnly(2024, 1, 1);
        
        //act
        var result = UserCalendar.Create(day);
        
        //assert
        result.Day.Value.ShouldBe(day);
    }
}