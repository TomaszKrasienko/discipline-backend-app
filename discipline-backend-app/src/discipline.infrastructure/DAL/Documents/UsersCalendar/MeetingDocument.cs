namespace discipline.infrastructure.DAL.Documents.UsersCalendar;

public class MeetingDocument : EventDocument, IDocument
{
    public TimeOnly TimeFrom { get; set; }
    public TimeOnly? TimeTo { get; set; }
    public string Platform { get; set; }
    public string Uri { get; set; }
    public string Place { get; set; }
}