namespace discipline.application.DTOs;

public class DailyProductivityDto
{
    public DateOnly Day { get; set; }
    public IReadOnlyList<ActivityDto> Activities { get; set; }
}