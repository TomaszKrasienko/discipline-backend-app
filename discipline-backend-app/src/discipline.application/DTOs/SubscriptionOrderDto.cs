namespace discipline.application.DTOs;

public class SubscriptionOrderDto
{
    public Ulid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Ulid SubscriptionId { get; set; }
    public bool StateIsCancelled { get; set; }
    public DateOnly? StateActiveTill { get; set; }
    public DateOnly? Next { get; set; }
    public string PaymentDetailsCardNumber { get; set; }
    public string PaymentDetailsCvvCode { get; set; }
    public int? Type { get; set; }
}