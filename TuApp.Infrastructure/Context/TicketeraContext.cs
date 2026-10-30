using Microsoft.EntityFrameworkCore;
using TuApp.Domain.Entities;

namespace TuApp.Infrastructure.Context;

public class TicketeraContext : DbContext
{
    public TicketeraContext(
        DbContextOptions<TicketeraContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Response> Responses => Set<Response>();


protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<User>(entity =>
    {
        entity.ToTable("users");
        entity.HasKey(e => e.UserId);

        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.Username).HasColumnName("username");
        entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
        entity.Property(e => e.Email).HasColumnName("email");
        entity.Property(e => e.CreatedAt).HasColumnName("created_at");

        entity.HasMany(e => e.Tickets)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);

        entity.HasMany(e => e.Responses)
            .WithOne(e => e.Responder)
            .HasForeignKey(e => e.ResponderId);

        entity.HasMany(e => e.UserRoles)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);
    });

    modelBuilder.Entity<Role>(entity =>
    {
        entity.ToTable("roles");
        entity.HasKey(e => e.RoleId);

        entity.Property(e => e.RoleId).HasColumnName("role_id");
        entity.Property(e => e.RoleName).HasColumnName("role_name");

        entity.HasMany(e => e.UserRoles)
            .WithOne(e => e.Role)
            .HasForeignKey(e => e.RoleId);
    });

    modelBuilder.Entity<UserRole>(entity =>
    {
        entity.ToTable("user_roles");
        entity.HasKey(e => new { e.UserId, e.RoleId });

        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.RoleId).HasColumnName("role_id");
        entity.Property(e => e.AssignedAt).HasColumnName("assigned_at");
    });

    modelBuilder.Entity<Ticket>(entity =>
    {
        entity.ToTable("tickets");
        entity.HasKey(e => e.TicketId);

        entity.Property(e => e.TicketId).HasColumnName("ticket_id");
        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.Title).HasColumnName("title");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.Status).HasColumnName("status");
        entity.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp without time zone");

        entity.Property(e => e.ClosedAt)
            .HasColumnName("closed_at")
            .HasColumnType("timestamp without time zone");

        entity.HasMany(e => e.Responses)
            .WithOne(e => e.Ticket)
            .HasForeignKey(e => e.TicketId);
    });

    modelBuilder.Entity<Response>(entity =>
    {
        entity.ToTable("responses");
        entity.HasKey(e => e.ResponseId);

        entity.Property(e => e.ResponseId).HasColumnName("response_id");
        entity.Property(e => e.TicketId).HasColumnName("ticket_id");
        entity.Property(e => e.ResponderId).HasColumnName("responder_id");
        entity.Property(e => e.Message).HasColumnName("message");
        entity.Property(e => e.CreatedAt).HasColumnName("created_at");
    });
}

}