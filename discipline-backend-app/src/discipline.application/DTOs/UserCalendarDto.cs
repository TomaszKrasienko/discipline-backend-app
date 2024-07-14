namespace discipline.application.DTOs;

public class UserCalendarDto
{
    public DateOnly Day { get; set; }
    public List<ImportantDateDto> ImportantDates { get; set; }
}