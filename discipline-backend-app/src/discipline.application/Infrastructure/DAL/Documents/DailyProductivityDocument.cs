using MongoDB.Bson.Serialization.Attributes;

namespace discipline.application.Infrastructure.DAL.Documents;

public class DailyProductivityDocument : IDocument
{
    [BsonElement("day")]
    [BsonId]
    public DateOnly Day { get; set; }

    [BsonElement("activities")]
    public IEnumerable<ActivityDocument> Activities { get; set; }
}