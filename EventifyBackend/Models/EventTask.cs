using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EventifyBackend.Models
{
    public class EventTasks
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Priority { get; set; } = "Low";

        public string Budget { get; set; } = string.Empty;

        public bool Completed { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public int EventId { get; set; }

        [JsonIgnore] // Prevent cycles during JSON serialization
        [ForeignKey("EventId")]
        public Event? Event { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? AssignedUser { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool Archived { get; set; } = false;
    }
}
