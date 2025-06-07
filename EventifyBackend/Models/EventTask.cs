using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventifyBackend.Models
{
    public class EventTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("priority")]
        public string Priority { get; set; } = "Low";

        [Column("assignedTo")]
        public string AssignedTo { get; set; } = "";

        [Column("budget")]
        public string Budget { get; set; } = "";

        [Column("completed")]
        public bool Completed { get; set; } = false;

        [Column("description")]
        public string? Description { get; set; }

        [Column("dueDate")]
        public DateTime? DueDate { get; set; }

        [Column("eventId")]
        public int EventId { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }

        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [Column("archived")]
        public bool Archived { get; set; } = false; // For archiving tasks
    }
}