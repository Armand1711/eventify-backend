using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventifyBackend.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        // other event properties
        public DateTime Date { get; set; }

        public bool Archived { get; set; } = false;

        [Column("userId")]
        public int UserId { get; set; }

        // Navigation property for tasks
        public List<EventTask> Tasks { get; set; } = new();
    }

    public class EventTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

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

        [Column("eventId")]
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("archived")]
        public bool Archived { get; set; } = false;

        [Column("userId")]
        public int UserId { get; set; }

        // Navigation property for user assigned to the task (if needed)
        // public User AssignedUser { get; set; } = null!;
    }
}
