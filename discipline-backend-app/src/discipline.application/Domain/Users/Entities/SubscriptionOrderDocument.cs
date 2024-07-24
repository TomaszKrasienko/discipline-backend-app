using MongoDB.Bson.Serialization.Attributes;

namespace discipline.application.Domain.Users.Entities;

public class SubscriptionOrderDocument
{
    [BsonId]
    [BsonElement("id")]
    public Guid Id { get; set; }
    
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
    
    [BsonElement("subscriptionId")]
    public Guid SubscriptionId { get; set; }
    
    [BsonElement("stateIsCancelled")]
    public bool StateIsCancelled { get; set; }
    
    [BsonElement("stateActiveTill")]
    public DateOnly StateActiveTill { get; set; }
    
    [BsonElement("next")]
    public DateOnly Next { get; set; }
    
    [BsonElement("paymentDetailsCardNumber")]
    public string PaymentDetailsCardNumber { get; set; }
    
    [BsonElement("paymentDetailsCvvCode")]
    public string PaymentDetailsCvvCode { get; set; }
}