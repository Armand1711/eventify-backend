using System.ComponentModel.DataAnnotations.Schema;

namespace EventifyBackend.Models
{
    public class EventTask
    {
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; } = string.Empty;

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
    }
}