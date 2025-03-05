namespace discipline.centre.users.application.Users.DTOs;

public sealed record SignUpDto(string Email, string Password, string FirstName, 
    string LastName);