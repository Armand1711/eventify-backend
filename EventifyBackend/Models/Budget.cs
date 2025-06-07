using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventifyBackend.Models
{
    public class Budget
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("category")]
        public string Category { get; set; } = string.Empty;

        [Column("amount")]
        public float Amount { get; set; }

        [Column("eventId")]
        public int EventId { get; set; }

        [Column("userId")]
        public int UserId { get; set; }

        // Optional: Add timestamps if you want
        // [Column("createdAt")]
        // public DateTime CreatedAt { get; set; }
        // [Column("updatedAt")]
        // public DateTime UpdatedAt { get; set; }
    }
}