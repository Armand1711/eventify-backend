namespace EventifyBackend.Dtos
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool Archived { get; set; }
        public List<EventTaskDto> Tasks { get; set; } = new();
    }

    public class EventTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Priority { get; set; } = "Low";
        public string Budget { get; set; } = string.Empty;
        public bool Completed { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
