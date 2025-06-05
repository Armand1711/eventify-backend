namespace EventifyBackend.Models;

public class User
{
    public int Id { get; set; }
    public string? Email { get; set; } // Made nullable to avoid CS8618
    public string? PasswordHash { get; set; } // Made nullable to avoid CS8618
}