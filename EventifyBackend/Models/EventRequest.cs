using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventifyBackend.Models
{
    public class EventRequest
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

        [Required]
        [Column("requester_name")]
        public string RequesterName { get; set; } = string.Empty;

        [Required]
        [Column("requester_email")]
        public string RequesterEmail { get; set; } = string.Empty;

        [Required]
        [Column("status")]
        public string Status { get; set; } = "Pending";

        [Column("userId")]
        public int? UserId { get; set; } // Nullable for unprocessed requests

        [ForeignKey("UserId")]
        public User? ProcessedByUser { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }

        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}