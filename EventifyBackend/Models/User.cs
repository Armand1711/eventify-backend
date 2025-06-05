using System.ComponentModel.DataAnnotations.Schema;

namespace EventifyBackend.Models;

public class User
{
    public int Id { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("password")]
    public string? PasswordHash { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }

    [Column("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}