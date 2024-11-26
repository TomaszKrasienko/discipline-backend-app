using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.users.infrastructure.DAL.Users.Documents;

internal sealed class PaidSubscriptionOrderDocument : SubscriptionOrderDocument
{
    [BsonElement("next")]
    public DateOnly Next { get; set; }
    
    [BsonElement("paymentDetailsCardNumber")]
    public required string PaymentToken { get; set; }
    
    [BsonElement]
    public int Type { get; set; }
}