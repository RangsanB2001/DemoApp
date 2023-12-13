using Microsoft.EntityFrameworkCore;

namespace AuthenticationAPI.Models;

public partial class RangsanContext : DbContext
{
    public RangsanContext(DbContextOptions<RangsanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.iduser).HasName("PRIMARY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
