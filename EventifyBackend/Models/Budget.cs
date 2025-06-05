using System.ComponentModel.DataAnnotations.Schema;

namespace EventifyBackend.Models
{
    public class Budget
    {
        public int Id { get; set; }

        [Column("category")]
        public string Category { get; set; } = string.Empty;

        [Column("amount")]
        public float Amount { get; set; }

        [Column("eventid")]
        public int EventId { get; set; }

        [Column("userid")]
        public int UserId { get; set; }
    }
}