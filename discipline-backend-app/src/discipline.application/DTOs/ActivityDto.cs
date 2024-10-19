namespace discipline.application.DTOs;

public class ActivityDto
{
    public Ulid Id { get; set; }
    public string Title { get; set; }
    public bool IsChecked { get; set; }
    public Ulid? ParentRuleId { get; set; }
}