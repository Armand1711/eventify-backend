using Microsoft.EntityFrameworkCore;

namespace EventifyBackend.Models
{
    public class EventifyDbContext : DbContext
    {
        public EventifyDbContext(DbContextOptions<EventifyDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventTask> EventTasks { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Archive> Archives { get; set; }
    }
}