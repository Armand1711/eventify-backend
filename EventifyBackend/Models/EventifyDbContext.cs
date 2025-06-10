using Microsoft.EntityFrameworkCore;

namespace EventifyBackend.Models;

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

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
        });

        // EventRequest configuration
        modelBuilder.Entity<EventRequest>(entity =>
        {
            entity.ToTable("EventRequests");
            entity.HasKey(er => er.Id);
            entity.Property(er => er.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(er => er.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(er => er.UserId).HasColumnName("userId");
        });

        // Event configuration
        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("Events");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.Tasks)
                  .WithOne(t => t.Event)
                  .HasForeignKey(t => t.EventId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Archived).HasDefaultValue(false);
        });

        // EventTasks configuration
        modelBuilder.Entity<EventTasks>(entity =>
        {
            entity.ToTable("EventTasks");
            entity.HasKey(et => et.Id);
            entity.Property(et => et.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(et => et.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasOne(et => et.Event)
                  .WithMany(e => e.Tasks)
                  .HasForeignKey(et => et.EventId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(et => et.AssignedUser)
                  .WithMany()
                  .HasForeignKey(et => et.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.Property(et => et.Archived).HasDefaultValue(false);
            entity.Property(et => et.Budget).HasColumnName("budget");
        });

        // Budget configuration
        modelBuilder.Entity<Budget>(entity =>
        {
            entity.ToTable("Budgets");
            entity.HasKey(b => b.Id);
        });

        // Archive configuration
        modelBuilder.Entity<Archive>(entity =>
        {
            entity.ToTable("Archives");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(a => a.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasOne(a => a.Event)
                  .WithMany()
                  .HasForeignKey(a => a.EventId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(a => a.User)
                  .WithMany()
                  .HasForeignKey(a => a.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}