using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.calendar.infrastructure.DAL.Documents;

[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(TimeEventDocument), typeof(ImportantDateEventDocument))]
internal abstract record BaseCalendarEventDocument : IDocument
{
    [BsonId]
    [BsonElement("eventId")]
    internal required string EventId { get; init; }
    
    [BsonElement("content")]
    internal required CalendarEventContentDocument Content { get; init; }
}