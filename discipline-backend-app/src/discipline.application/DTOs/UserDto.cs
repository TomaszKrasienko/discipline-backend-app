namespace discipline.application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Status { get; set; }
    public SubscriptionOrderDto SubscriptionOrder { get; set; }
}