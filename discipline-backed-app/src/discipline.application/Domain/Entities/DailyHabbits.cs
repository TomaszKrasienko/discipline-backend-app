namespace discipline.application.Domain.Entities;

public sealed class DailyHabits
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public List<ActivityItem> ActivityItems { get; set; }
}