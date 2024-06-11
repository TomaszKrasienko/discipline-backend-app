namespace discipline.application.Domain;

public class ActivityItem
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool Checked { get; set; }
    public Guid ParentId { get; set; }
}