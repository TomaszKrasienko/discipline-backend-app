namespace discipline.centre.users.application.Users.DTOs.Endpoints;

public sealed record SignUpRequest(string Email, string Password, string FirstName, string LastName);