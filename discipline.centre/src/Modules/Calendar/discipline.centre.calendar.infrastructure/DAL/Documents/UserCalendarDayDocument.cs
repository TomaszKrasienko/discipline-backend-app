using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.calendar.infrastructure.DAL.Documents;

internal sealed record UserCalendarDayDocument : IDocument
{
    [BsonId] 
    [BsonElement("userCalendarId")]
    internal required string UserCalendarId { get; init; }
    
    [BsonElement("userId")]
    internal required string UserId { get; init; }

    [BsonElement("day")]
    internal DateOnly Day { get; init; }

    [BsonElement("events")]
    internal required IReadOnlyCollection<BaseCalendarEventDocument> Events { get; init; }
}