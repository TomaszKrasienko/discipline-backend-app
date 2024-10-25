using MongoDB.Bson.Serialization.Attributes;

namespace discipline.infrastructure.DAL.Documents.UsersCalendar;

[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(MeetingDocument), typeof(ImportantDateDocument), typeof(CalendarEventDocument))]
public class EventDocument : IDocument
{    
    [BsonElement("id")]
    [BsonId]
    public string Id { get; set; }
    
    [BsonElement("title")]
    public string Title { get; set; }
}