namespace EventifyBackend.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public float Amount { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
    }
}