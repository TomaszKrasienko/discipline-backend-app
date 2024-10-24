namespace discipline.application.DTOs;

public class SubscriptionDto
{
    public Ulid Id { get; set; }
    public string Title { get; set; }
    public decimal PricePerMonth { get; set; }
    public decimal PricePerYear { get; set; }
    public bool IsPaid { get; set; }
    public List<string> Features { get; set; }
}