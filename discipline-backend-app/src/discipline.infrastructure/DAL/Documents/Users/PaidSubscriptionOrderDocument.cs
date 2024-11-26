using MongoDB.Bson.Serialization.Attributes;

namespace discipline.infrastructure.DAL.Documents.Users;

internal sealed class PaidSubscriptionOrderDocument : SubscriptionOrderDocument
{
    [BsonElement("next")]
    public DateOnly Next { get; set; }
    
    [BsonElement("paymentDetailsCardNumber")]
    public string PaymentDetailsCardNumber { get; set; }
    
    [BsonElement("paymentDetailsCvvCode")]
    public string PaymentDetailsCvvCode { get; set; }
    
    [BsonElement]
    public int Type { get; set; }
}