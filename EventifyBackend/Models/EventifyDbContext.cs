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
        public DbSet<EventRequest> EventRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Entity<User>().ToTable("Users").HasKey(u => u.Id); // Match your User model
            modelBuilder.Entity<EventRequest>().ToTable("EventRequests");
   
        }
    }
}