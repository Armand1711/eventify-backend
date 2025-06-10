using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventifyBackend.Models;

public class EventTasks
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("priority")]
    public string? Priority { get; set; }

    [Column("budget")]
    public string? Budget { get; set; } // Changed from decimal? to string?

    [Column("completed")]
    public bool Completed { get; set; } = false;

    [Column("description")]
    public string? Description { get; set; }

    [Column("dueDate")]
    public DateTime? DueDate { get; set; }

    [Required]
    [Column("eventId")]
    public int EventId { get; set; }

    [ForeignKey("EventId")]
    public Event? Event { get; set; }

    [Required]
    [Column("userId")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User? AssignedUser { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }

    [Column("updatedAt")]
    public DateTime UpdatedAt { get; set; }

    [Column("archived")]
    public bool Archived { get; set; } = false;
}