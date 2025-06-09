using Microsoft.EntityFrameworkCore;

namespace EventifyBackend.Models
{
    public class EventifyDbContext : DbContext
    {
        public EventifyDbContext(DbContextOptions<EventifyDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<EventTasks> EventTasks { get; set; } = null!;
        public DbSet<Budget> Budgets { get; set; } = null!;
        public DbSet<Archive> Archives { get; set; } = null!;
        public DbSet<EventRequest> EventRequests { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);
            });

            modelBuilder.Entity<EventRequest>(entity =>
            {
                entity.ToTable("EventRequests");
                entity.HasKey(er => er.Id);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Events");
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.Tasks)
                      .WithOne(t => t.Event)
                      .HasForeignKey(t => t.EventId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EventTasks>(entity =>
            {
                entity.ToTable("EventTasks");
                entity.HasKey(et => et.Id);

                entity.HasOne(et => et.AssignedUser)
                      .WithMany()
                      .HasForeignKey(et => et.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Budget>(entity =>
            {
                entity.ToTable("Budgets");
                entity.HasKey(b => b.Id);
            });

            modelBuilder.Entity<Archive>(entity =>
            {
                entity.ToTable("Archives");
                entity.HasKey(a => a.Id);
            });
        }
    }
}
