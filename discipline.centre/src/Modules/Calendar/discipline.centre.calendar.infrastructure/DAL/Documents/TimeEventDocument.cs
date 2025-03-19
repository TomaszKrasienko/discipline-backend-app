using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.calendar.infrastructure.DAL.Documents;

internal sealed record TimeEventDocument : BaseCalendarEventDocument, IDocument
{
    [BsonElement("timeSpan")]
    internal required TimeEventTimeSpanDocument TimeSpan { get; init; }
}