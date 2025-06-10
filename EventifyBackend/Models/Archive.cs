using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventifyBackend.Models
{
    public class Archive
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("eventId")]
        public int EventId { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("date")] // Remove [Required] to allow null
        public DateTime? Date { get; set; } // Changed to nullable

        [Required]
        [Column("userId")]
        public int UserId { get; set; }

        [Required]
        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}