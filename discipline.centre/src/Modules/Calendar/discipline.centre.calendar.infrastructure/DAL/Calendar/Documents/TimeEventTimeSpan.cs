using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.calendar.infrastructure.DAL.Calendar.Documents;

internal sealed record TimeEventTimeSpan : IDocument 
{
    [BsonElement("from")]
    internal TimeOnly From { get; init; }
    
    [BsonElement("to")]
    internal TimeOnly? To { get; init; }
}