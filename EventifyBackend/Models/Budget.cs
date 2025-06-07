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

        [Column("taskId")]
        public int TaskId { get; set; } // Link to EventTask

        [Column("userId")]
        public int UserId { get; set; }
    }
}