using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.calendar.infrastructure.DAL.Calendar.Documents;

internal sealed record TimeEventDocument : BaseCalendarEventDocument, IDocument
{
    [BsonElement("timeSpan")]
    internal required TimeEventTimeSpan TimeSpan { get; init; }
}