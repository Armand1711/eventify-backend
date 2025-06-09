using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventifyBackend.Models
{
    public class EventTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column("priority")]
        public string Priority { get; set; } = "Low";

        [Column("budget")]
        public string Budget { get; set; } = string.Empty;

        [Column("completed")]
        public bool Completed { get; set; } = false;

        [Column("description")]
        public string? Description { get; set; }

        [Column("dueDate")]
        public DateTime? DueDate { get; set; }

        // Foreign key to Event
        [Required]
        [Column("eventId")]
        public int EventId { get; set; }

        // Navigation property for Event (optional but recommended)
        [ForeignKey("EventId")]
        public Event? Event { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("archived")]
        public bool Archived { get; set; } = false;

        // Foreign key for User assigned to task
        [Required]
        [Column("userId")]
        public int UserId { get; set; }

        // Navigation property for assigned User
        [ForeignKey("UserId")]
        public User AssignedUser { get; set; } = null!;
    }
}
