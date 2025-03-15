Bnamespace discipline.infrastructure.DAL.Documents.UsersCalendar;

internal sealed class CalendarEventDocument : EventDocument, IDocument
{
    public TimeOnly TimeFrom { get; set; }
    public TimeOnly? TimeTo { get; set; }
    public string Action { get; set; }
}