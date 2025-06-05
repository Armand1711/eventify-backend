namespace EventifyBackend.Models
{
    public class Archive
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}