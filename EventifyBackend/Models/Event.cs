using System.ComponentModel.DataAnnotations;

namespace EventifyBackend.Models
{
    public class Event
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public ICollection<EventTasks> Tasks { get; set; } = new List<EventTasks>();

        public User? User { get; set; }
    }
}
