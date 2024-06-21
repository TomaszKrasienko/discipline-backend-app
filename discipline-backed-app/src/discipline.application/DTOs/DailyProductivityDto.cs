namespace discipline.application.DTOs;

public class DailyProductivityDto
{
    public DateTime Day { get; set; }
    public IReadOnlyList<ActivityDto> Activities { get; set; }
}