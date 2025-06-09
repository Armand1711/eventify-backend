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
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("date")]
        public DateTime Date { get; set; }

        // Foreign key to User
        [Required]
        [Column("userId")]
        public int UserId { get; set; }

        // Optional: Navigation property to User
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }

        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [Column("archived")]
        public bool Archived { get; set; } = false;

        // Navigation property for related EventTasks
        public virtual ICollection<EventTask> Tasks { get; set; }

        public Event()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Tasks = new List<EventTask>();
        }
    }
}
