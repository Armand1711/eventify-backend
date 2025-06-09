using Microsoft.EntityFrameworkCore;

namespace EventifyBackend.Models
{
    public class EventifyDbContext : DbContext
    {
        public EventifyDbContext(DbContextOptions<EventifyDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<EventTask> EventTasks { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Archive> Archives { get; set; }
        public DbSet<EventRequest> EventRequests { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<User>().ToTable("Users").HasKey(u => u.Id);
            modelBuilder.Entity<EventRequest>().ToTable("EventRequests");

            // Configure relationship between EventTask and User
            modelBuilder.Entity<EventTask>()
                .HasOne(et => et.AssignedUser)
                .WithMany()
                .HasForeignKey(et => et.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete (optional)

            // Configure relationship between Event and EventTask (one-to-many)
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Tasks)
                .WithOne(t => t.Event)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete tasks when event is deleted
        }
    }
}
