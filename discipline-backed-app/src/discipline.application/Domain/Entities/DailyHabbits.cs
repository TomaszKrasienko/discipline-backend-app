namespace discipline.application.Domain;

public sealed class DailyHabits
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public List<ActivityItem> ActivityItems { get; set; }
}