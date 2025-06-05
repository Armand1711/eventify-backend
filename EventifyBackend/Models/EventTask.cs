namespace EventifyBackend.Models
   {
    public class EventTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
           public bool IsCompleted { get; set; }
       }
   }