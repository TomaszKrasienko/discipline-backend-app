using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;

internal sealed class SubscriptionDocument : IDocument
{
    [BsonId]
    [BsonElement("id")]
    public required string Id { get; set; }
    
    [BsonElement("title")]
    public required string Title { get; set; }
    
    [BsonElement("pricePerMonth")]
    public decimal PricePerMonth { get; set; }
    
    [BsonElement("pricePerYear")]
    public decimal PricePerYear { get; set; }
    
    [BsonElement("isPaid")]
    public bool IsPaid { get; set; }
    
    [BsonElement("features")]
    public required List<string> Features { get; set; }
}