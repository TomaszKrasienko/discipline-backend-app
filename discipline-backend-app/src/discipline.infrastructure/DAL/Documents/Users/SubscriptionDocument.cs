using MongoDB.Bson.Serialization.Attributes;

namespace discipline.infrastructure.DAL.Documents.Users;

public class SubscriptionDocument : IDocument
{
    [BsonId]
    [BsonElement("id")]
    public string Id { get; set; }
    
    [BsonElement("title")]
    public string Title { get; set; }
    
    [BsonElement("pricePerMonth")]
    public decimal PricePerMonth { get; set; }
    
    [BsonElement("pricePerYear")]
    public decimal PricePerYear { get; set; }
    
    [BsonElement("isPaid")]
    public bool IsPaid { get; set; }
    
    [BsonElement("features")]
    public List<string> Features { get; set; }
}