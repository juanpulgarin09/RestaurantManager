using Microsoft.EntityFrameworkCore;
using RestaurantManager.Domain.Entities;

namespace RestaurantManager.DataAccess.Context;

public class RestaurantDbContext : DbContext
{
    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
        : base(options) { }

    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<Table> Tables => Set<Table>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Restaurant ──
        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name)
                  .IsRequired()
                  .HasMaxLength(150);
            entity.Property(r => r.Address)
                  .IsRequired()
                  .HasMaxLength(250);
            entity.Property(r => r.Phone)
                  .IsRequired()
                  .HasMaxLength(20);
            entity.Property(r => r.Email)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(r => r.CreatedAt).IsRequired();
            entity.Property(r => r.UpdatedAt).IsRequired(false);

            entity.HasIndex(r => r.Name).IsUnique();
        });

        // ── Table ──
        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Number).IsRequired();
            entity.Property(t => t.Capacity).IsRequired();
            entity.Property(t => t.Status)
                  .IsRequired()
                  .HasConversion<string>();
            entity.Property(t => t.CreatedAt).IsRequired();
            entity.Property(t => t.UpdatedAt).IsRequired(false);

            // 1:N Restaurant → Tables
            entity.HasOne(t => t.Restaurant)
                  .WithMany(r => r.Tables)
                  .HasForeignKey(t => t.RestaurantId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Número de mesa único por restaurante
            entity.HasIndex(t => new { t.RestaurantId, t.Number }).IsUnique();
        });

        // ── Customer ──
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.FullName)
                  .IsRequired()
                  .HasMaxLength(150);
            entity.Property(c => c.Email)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(c => c.Phone)
                  .IsRequired()
                  .HasMaxLength(20);
            entity.Property(c => c.CreatedAt).IsRequired();
            entity.Property(c => c.UpdatedAt).IsRequired(false);

            entity.HasIndex(c => c.Email).IsUnique();
        });

        // ── MenuItem ──
        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Name)
                  .IsRequired()
                  .HasMaxLength(150);
            entity.Property(m => m.Description)
                  .HasMaxLength(500);
            entity.Property(m => m.Price)
                  .IsRequired()
                  .HasColumnType("decimal(10,2)");
            entity.Property(m => m.Category)
                  .IsRequired()
                  .HasConversion<string>();
            entity.Property(m => m.IsAvailable).IsRequired();
            entity.Property(m => m.CreatedAt).IsRequired();
            entity.Property(m => m.UpdatedAt).IsRequired(false);
        });

        // ── Reservation ──
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.ReservationDate).IsRequired();
            entity.Property(r => r.GuestCount).IsRequired();
            entity.Property(r => r.Status)
                  .IsRequired()
                  .HasConversion<string>();
            entity.Property(r => r.Notes).HasMaxLength(500);
            entity.Property(r => r.CreatedAt).IsRequired();
            entity.Property(r => r.UpdatedAt).IsRequired(false);

            // 1:N Customer → Reservations (Restrict: no borrar cliente con reservas)
            entity.HasOne(r => r.Customer)
                  .WithMany(c => c.Reservations)
                  .HasForeignKey(r => r.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);

            // 1:N Table → Reservations (Restrict: no borrar mesa con reservas)
            entity.HasOne(r => r.Table)
                  .WithMany(t => t.Reservations)
                  .HasForeignKey(r => r.TableId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Order ──
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.TotalAmount)
                  .IsRequired()
                  .HasColumnType("decimal(10,2)");
            entity.Property(o => o.CreatedAt).IsRequired();
            entity.Property(o => o.UpdatedAt).IsRequired(false);

            // 1:1 Reservation → Order
            entity.HasOne(o => o.Reservation)
                  .WithOne(r => r.Order)
                  .HasForeignKey<Order>(o => o.ReservationId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Índice único en ReservationId garantiza relación 1:1
            entity.HasIndex(o => o.ReservationId).IsUnique();
        });

        // ── OrderItem ──
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(oi => oi.Id);
            entity.Property(oi => oi.Quantity).IsRequired();
            entity.Property(oi => oi.UnitPrice)
                  .IsRequired()
                  .HasColumnType("decimal(10,2)");
            entity.Property(oi => oi.CreatedAt).IsRequired();
            entity.Property(oi => oi.UpdatedAt).IsRequired(false);

            // N:M Order ↔ MenuItem via OrderItem
            entity.HasOne(oi => oi.Order)
                  .WithMany(o => o.OrderItems)
                  .HasForeignKey(oi => oi.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(oi => oi.MenuItem)
                  .WithMany(m => m.OrderItems)
                  .HasForeignKey(oi => oi.MenuItemId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Un mismo ítem no puede repetirse en el mismo pedido
            entity.HasIndex(oi => new { oi.OrderId, oi.MenuItemId }).IsUnique();
        });
    }
}