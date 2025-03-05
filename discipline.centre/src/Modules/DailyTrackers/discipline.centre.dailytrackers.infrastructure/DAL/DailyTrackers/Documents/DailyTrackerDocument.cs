using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;

public sealed record DailyTrackerDocument : IDocument
{
    [BsonId]
    [BsonElement("dailyTrackerId")]
    public required string DailyTrackerId { get; init; }
    
    [BsonElement("day")]
    public DateOnly Day { get; init; }
    
    [BsonElement("userId")]
    public required string UserId { get; init; }
    
    [BsonElement("activities")]
    public required IEnumerable<ActivityDocument> Activities { get; init; }
}