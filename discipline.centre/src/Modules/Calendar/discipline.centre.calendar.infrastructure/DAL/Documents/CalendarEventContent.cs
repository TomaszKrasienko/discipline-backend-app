using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.calendar.infrastructure.DAL.Documents;

internal sealed record CalendarEventContentDocument : IDocument
{
    [BsonElement("title")] 
    internal required string Title { get; init; }

    [BsonElement("description")] 
    internal string? Description { get; init; }
}