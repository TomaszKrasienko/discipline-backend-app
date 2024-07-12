namespace discipline.application.Infrastructure.DAL.Documents.UsersCalendar;

public class CalendarEventDocument : EventDocument, IDocument
{
    public TimeOnly From { get; set; }
    public TimeOnly? TimeTo { get; set; }
    public string Action { get; set; }
}