using System.ComponentModel.DataAnnotations.Schema;

namespace EventifyBackend.Models
{
    public class Archive
    {
        public int Id { get; set; }

        [Column("eventId")]
        public int EventId { get; set; }

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("userId")]
        public int UserId { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }

        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}