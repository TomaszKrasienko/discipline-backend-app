using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.calendar.infrastructure.DAL.Calendar.Documents;

[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(TimeEventDocument), typeof(ImportantDateEventDocument))]
internal abstract record BaseCalendarEventDocument : IDocument
{
    [BsonElement("content")]
    internal required CalendarEventContent Content { get; init; }
}